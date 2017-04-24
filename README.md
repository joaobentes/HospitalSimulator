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

### For Docker

To run the Hospital Simulator base image, you should install Docker. Please refer to here to get instructions to install Docker on Windows, MacOS and Linux.

The following command will get you running a container with the Hospital Simulator:

```shell
docker run jbentes/hospital-simulator:0.1
```

## API Reference Documentation

**Register a patient**
----

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
          u
    }
    ```

* **Success Response:**
  
  * **Code:** 200 <br />
    **Content:** `{ id : 12 }`
 
* **Error Response:**

  * **Code:** 422 UNPROCESSABLE ENTRY <br />
    **Content:** `{ error : "Email Invalid" }`

**Get all patients**
----

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
    **Content:** `{ id : 12 }`
 
* **Error Response:**

  * **Code:** 422 UNPROCESSABLE ENTRY <br />
    **Content:** `{ error : "Email Invalid" }`

**Get all consultations**
----

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
    **Content:** `{ id : 12 }`
 
* **Error Response:**

  * **Code:** 422 UNPROCESSABLE ENTRY <br />
    **Content:** `{ error : "Email Invalid" }`