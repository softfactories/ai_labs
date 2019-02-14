# This program analyze the given text for language, sentiments, keyphrases and entities 
# with REST-API for Text Analyzer of the Cognitive Services
import TextAnalyzerEx as experiments

# TODO: (1) Replace <Subscription Key> (aka API-KEY) with your valid subscription key.
subscription_key = "<Subscription Key>"
assert subscription_key
headers = {'Ocp-Apim-Subscription-Key': subscription_key }

# You must use the same region in your REST call as you used to get your
# subscription keys. For example, if you got your subscription keys from
# West Europe, replace "westcentralus" in the azureRegion-Variable below with "westeurope".
#
# Free trial subscription keys are generated in the "westcentralus" region.
# If you use a free trial subscription key, you shouldn't need to change
# this region.

# TODO: (2) assign actual azure region here:
azureRegion = "westcentralus"
# azureRegion = "westeurope" 
# ... etc.
text_base_url = "https://" + azureRegion + ".api.cognitive.microsoft.com/text/analytics/v2.0/"

# Experiment 1. - analyse language
experiments.Experiment_1(headers, text_base_url)

# Experiment 2. - analyse sentiments 
experiments.Experiment_2(headers, text_base_url)

# Experiment 3. - extract keyphrases
experiments.Experiment_3(headers, text_base_url)

# Experiment 4. - entities identification
experiments.Experiment_4(headers, text_base_url)
