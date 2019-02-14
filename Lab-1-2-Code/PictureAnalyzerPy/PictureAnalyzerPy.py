# coding=utf-8

# This program does analyze the local and remote pictures 
# with REST-API for Computer Vision of the Cognitive Services

import requests
# If you are using a Jupyter notebook, uncomment the following line.
#%matplotlib inline
import matplotlib.pyplot as plt
from PIL import Image
from io import BytesIO

# TODO: (1) Replace <Subscription Key> (aka API-KEY) with your valid subscription key.
subscription_key = "<Subscription Key>"
assert subscription_key

import re
def FindUrl(string): 
    # findall() has been used  
    # with valid conditions for urls in string 
    url = re.findall("http[s]?://(?:[a-zA-Z]|[0-9]|[$-_@.&+]|[!*\(\), ]|(?:%[0-9a-fA-F][0-9a-fA-F]))+", string) 
    return url

# You must use the same region in your REST call as you used to get your
# subscription keys. For example, if you got your subscription keys from
# East US, replace "westcentralus" in the azureRegion-Variable below with "eastus".
#
# Free trial subscription keys are generated in the "westcentralus" region.
# If you use a free trial subscription key, you shouldn't need to change
# this region.

# TODO: (2) assign actual azure region here:
azureRegion = "westcentralus"
# azureRegion = "eastus" 
# ... etc.

vision_base_url = "https://" + azureRegion + ".api.cognitive.microsoft.com/vision/v2.0/"
analyze_url = vision_base_url + "analyze"

# Set image_path to the local path of an image that you want to analyze.
image_path = input("Enter the path to the image you wish to analyze: ")

# parse image path
image_url = FindUrl(image_path)
is_not_url = len(image_url) < 1

headers = None
response = None
params = None

if is_not_url:
    # Analyze local file
    # set api-key
    headers    = {'Ocp-Apim-Subscription-Key': subscription_key,
                  'Content-Type': 'application/octet-stream'}
    # set params
    params     = {'visualFeatures': 'Categories,Description,Color,Tags,Adult'}
    # Read the image into a byte array
    data_local = open(image_path, "rb").read()
    # query Computer Vision service on Azure 
    response = requests.post(analyze_url, headers=headers, params=params, data=data_local)
else:
    # Analyze a file from the Web
    headers = {'Ocp-Apim-Subscription-Key': subscription_key }
    # set api-key
    params  = {'visualFeatures': 'Categories,Description,Color'}
    # set image url
    data_by_url = {'url': image_path}
    # query Computer Vision service on Azure 
    response = requests.post(analyze_url, headers=headers, params=params, json=data_by_url)
    
response.raise_for_status()

# The 'analysis' object contains various fields that describe the image. The most
# relevant caption for the image is obtained from the 'description' property.
analysis = response.json()
print(analysis)
image_caption = analysis["description"]["captions"][0]["text"].capitalize()

image = None
if is_not_url:
    image = Image.open(BytesIO(data_local))
else:
    image = Image.open(BytesIO(requests.get(image_path).content))

# Display the image and overlay it with the caption.
plt.imshow(image)
plt.axis("off")
_ = plt.title(image_caption, size="x-large", y=-0.1)
plt.show()

 