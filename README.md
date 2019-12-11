# Experiment #101 - Organ Transportation
#### Using Blockchain & IoT During Organ Transportation.

------------

###### Project Date: 
- May - August 2019

###### Services: 
- Azure IoT Central 
- Azure IoT Hub 
- Azure Event Hubs 
- Azure Functions 
- Azure Logic Apps 
- Azure Blockchain Workbench

###### Technology used: 
- Internet of Things
- Blockchain

------------

## About 
This Experiment researches the possibilities of combining Internet of Things (IoT) and Blockchain for transportation of volatile goods. IoT allows us to communicate with devices and collect ambient measurements like temperature and humidity from different sensors. The different components involved send and receive data and the Blockchain structure stores, transmits and verifies this data with integrity and in a secure way.

## Idea
The experiment attempts to simulate the real time monitoring of environmental conditions in the transportation of volatile goods from one point to another, verifying at all times that the obtained values falls within the established security parameters.

## Utility
For this experiment we chose the case of organ transport for a transplant surgery. To make sure that the donated organ is always in perfect conditions, it is very important to have continuous monitoring of the environment.

In this case, the organ transport refrigeration system will have a temperature measuring sensor. The device will send real time readings to verify that the transporting conditions of the donated organ are suitable until the surgery.

## Process
First, the MxChip AZ3166 device connects to Microsoft Azure thru Wi-Fi. Then, we can collect all data readings by using Azure IoT Central and Azure IoT Hubs. After that, the data are sent to Azure Event Hubs and finally, they are sent to the main application.

Also, we set up an Azure Blockchain Workbench application which receives the measurements after passing through Azure Functions and Azure Logic Apps and it reports the status to the agents in real time. At this point, the agents can perform some predefined actions and the application registers every step to ensure the safety of it.

## Links

[Experiment: Using Blockchain & IoT During Organ Transportation](https://www.idiwork.com/projects/experiment-101/)

[Architecture Diagram: Services and Resources](https://www.idiwork.com/experiment-101-architectural-diagram/)

[Step by Step 1: How to set up an IoT device in Azure IoT Hub](https://www.idiwork.com/step-by-step-how-to-set-up-an-iot-device-in-azure-iot-hub/)

[Step by Step 2: How to create a Blockchain Workbench App](https://www.idiwork.com/how-to-create-a-blockchain-workbench-app/)

[Step by Step 3: How to create an Azure Function App to record telemetry readings](https://www.idiwork.com/experiment-101-how-to-create-and-azure-function-app-to-record-telemtry-readings/)
