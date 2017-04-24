# Hospital Simulator

A Web Service API to register patients and to schedule consultations in a Hospital.
This solution was developed using [.NET Core 1.1](https://www.microsoft.com/net/core).

## How to build?

### From source code

Before you can Hospital Simulator from source code, you must install .NET Core 1.1. Please follow the instructions [here](https://www.microsoft.com/net/core) to install .NET Core 1.1 on Windows, MacOS and Linux.

Once the .Net Core 1.1 is installed, fetch the source code from GitHub:

```shell
git clone https://github.com/joaobentes/HospitalSimulator.git
```

Then, go into the main project folder and fetch the required libraries:

```shell
dotnet restore
```

Finally, start the server:

```shell
dotnet run
```

The Web API will be available on the port 5000. For example: `http://localhost:5000`.

## API Reference Documentation

### Register a patient

* **URL**

  /patients

* **Method:**
  
  `POST`
  
*  **URL Params**

   None

* **Data Params**

    **Example**
    ```json
    {
    	"name": "John Doe",
    	"condition": "Cancer.Head&Neck"
    }
    ```

* **Success Response:**
  
  * **Code:** 200 <br />
    **Content:** 
    ```json
    {
      "consultationID": "b451bff9-0601-48bf-94ed-3cc4f9c953b2",
      "registrationDate": "2017-04-23T23:40:02.576276+02:00",
      "consultationDate": "2017-05-01T08:00:00+02:00",
      "patientID": "bf0982f5-2168-4b95-93ef-1e05dff055a5",
      "patient": {
        "PatientID": "bf0982f5-2168-4b95-93ef-1e05dff055a5",
        "Name": "John Doe",
        "Condition": "Cancer.Head&Neck"
      },
      "doctorID": "b004b101-31ac-4ffa-b476-d3be59047f48",
      "doctor": {
        "name": "Peter",
        "roles": [
          "Oncologist",
          "GeneralPractitioner"
        ]
      },
      "treatmentRoomName": "Two",
      "treatmentRoom": {
        "Name": "Two",
        "MachineName": "Varian",
        "Machine": {
          "Name": "Varian",
          "Capability": "Advanced"
        }
      }
    }
    ```
    
* **Error Response:**

  * **Code:** 400 BAD REQUEST <br />
    **Content:** 
    `{ error : "Invalid Patient Condition" }` OR `{ error : "Invalid Patient Data" }` OR `{ error = "Invalid Consultation object" }`
    
* **Notes:**

  The version 0.1 only supports three types of patient's condition: (1) Cancer.Head&Neck, (2) Cancer.Breast, (3) Flue.

### Get all patients

* **URL**

  /patients

* **Method:**
  
  `GET`
  
*  **URL Params**

   None

* **Data Params**

    None

* **Success Response:**
  
  * **Code:** 200 <br />
    **Content:**
    ```json
    [
      {
        "patientID": "3cb8f5ac-e005-44eb-8878-e3b836ce24b4",
        "name": "John Doe",
        "condition": "Cancer.Head&Neck"
      },
      {
        "patientID": "88772d40-f246-45d3-af66-555c177c6d47",
        "name": "John Smith",
        "condition": "Flue"
      },
      {
        "patientID": "6fa2dab9-c22c-41bd-b322-03a4071c14c9",
        "name": "Maria Smith",
        "condition": "Cancer.Breast"
      }
    ]
    ```
 
* **Error Response:**
  None

### Get all consultations

* **URL**

  /consultations

* **Method:**
  
  `GET`
  
*  **URL Params**

   None

* **Data Params**

    None

* **Success Response:**
  
  * **Code:** 200 <br />
    **Content:**
    ```json
    [
      {
        "consultationID": "d4f0b5cf-e61b-4289-aa6a-80036db54f12",
        "registrationDate": "2017-04-23T23:39:32.583086+02:00",
        "consultationDate": "2017-04-24T08:00:00+02:00",
        "patientID": "3cb8f5ac-e005-44eb-8878-e3b836ce24b4",
        "patient": {
          "PatientID": "3cb8f5ac-e005-44eb-8878-e3b836ce24b4",
          "Name": "Barna",
          "Condition": "Flue"
        },
        "doctorID": "5ca8ffd8-0744-4abf-a3cf-00fe71530f5e",
        "doctor": {
          "name": "Anna",
          "roles": [
            "GeneralPractitioner"
          ]
        },
        "treatmentRoomName": "Four",
        "treatmentRoom": {
          "Name": "Four",
          "MachineName": "",
          "Machine": null
        }
      },
      {
        "consultationID": "0c474375-b898-4f0f-888b-8ade813d4b29",
        "registrationDate": "2017-04-23T23:39:34.718341+02:00",
        "consultationDate": "2017-04-24T08:00:00+02:00",
        "patientID": "88772d40-f246-45d3-af66-555c177c6d47",
        "patient": {
          "PatientID": "88772d40-f246-45d3-af66-555c177c6d47",
          "Name": "Barna",
          "Condition": "Flue"
        },
        "doctorID": "b004b101-31ac-4ffa-b476-d3be59047f48",
        "doctor": {
          "name": "Peter",
          "roles": [
            "Oncologist",
            "GeneralPractitioner"
          ]
        },
        "treatmentRoomName": "Five",
        "treatmentRoom": {
          "Name": "Five",
          "MachineName": "",
          "Machine": null
        }
      }
    ]
    ```

* **Error Response:**
  None
