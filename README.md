# AI Labs & Demos
Demo-Projects and Labs for AI Courses from [Software Factories](http://www.software-factories.de/)

## Abstract
This repo contains sample lab-projects for the Course - ["AI, Machine and DeepLearning"](https://ki2019.eventbrite.de)

## The Labs List

 1. Lab N° 1.2 - Azure Cognitive Services
 2. Lab N° 2   - Machine Learning Services of Amazon AWS
 3. Lab N° 3   - Machine Learning Services of Google Cloud
 4. Lab N° 4   - Image binary classification with Convolutional Neural Network (CNN) in Python

## Experiment's Build and Run instructions

### Lab N° 1.2 - Azure Cognitive Services

1. Replace `<Subscription Key>` in the relevant Programmcode with your actual subscription key (aka API-Key) - i.e. `subscription_key = "jsdkas212321....ssdaskhddh"`
   You can get your trial keys from [here](https://azure.microsoft.com/try/cognitive-services/) or from your [Azure Portal](https://portal.azure.com).

2. Assign the actual Azure region in the relevant Programmcode where your cognitive service is running within - i.e. `azureRegion = "westcentralus"`

   You must use the same region in your REST call as you used to get your
   subscription keys. For example, if you got your subscription keys from
   East US, replace "westcentralus" with "eastus".

   Free trial subscription keys are generated in the "westcentralus" region.
   If you use a free trial subscription key, you shouldn't need to change
   this region.

 3. Further Informations and Details see on the Lab PowerPoint slides and [Documentation of Azure Cognitive Services](https://docs.microsoft.com/de-de/azure/cognitive-services/)



### Build and Run the Experiments (Visual Studio)
 1. Starting in the folder where you clone the repository
 
 2. Start Microsoft Visual Studio 2017 and select `File > Open > Project/Solution`.
 
 3. Double-click the Visual Studio 2017 Solution (.sln) file.

 4. Press Ctrl+Shift+B, or select `Build > Build Solution`.
 
 5. If needed use NuGet Packet-Manager to solve projects dependecies (see details in PowerPoint presentation slides for each Lab)
 
 6. Run the selected solution project (F5 or Ctrl+F5)
 
### Build and Run the Experiments (Eclipse)
 1. Start Eclipse IDE.
 
 2. Switch the workspace to where you clone the repository `File > Switch Workspace > Other`.
 
 3. Re-Start Eclipse IDE.

 4. Select/Open relevant Java-file (i.e. Main.java).
 
 5. Run selected Java-programm (i.e. via Menu 'Run > Run' or Ctrl+F11)

## Developer Code of Conduct
This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/).
