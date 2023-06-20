# AgriPen

AgriPen is an agriculture research app designed to collect telemetry data from IoT sensors and conduct analysis (possibly with AI models) to help farmers increase their production.

## Components

* AgriPen Backend, main API server using ASP.NET Core
* AgriPen Frontend, React + Vite based web app
* AgriPen Worker, Python-based server to run predictions using TensorFlow
* AgriPen Nginx, reverse proxy for backend and frontend
* AgriPen Mobile, .NET MAUI based app
* AgriPen ESP32, Arduino code to push telemetry data from sensors to AgriPen Mobile through Bluetooth.

## Building

### Building the mobile app

Clone this repo and open the solution on the `agripen-mobile` directory. You'll need VS 2022 with Mobile development workload (MAUI) to build this solution.

### Building everything else

I suggest building the apps in Linux, but you definitely can build it in Windows too! (I hate NodeJS on Windows)

You can build AgriPen using just Docker or for development purposes, you can setup a local environment.

Docker route:

1. Clone this repo
2. Install [Docker](https://docs.docker.com/get-docker/)
3. Run `deploy-all.sh`

Development route:

1. Clone this repo
2. Install [.NET](https://learn.microsoft.com/en-us/dotnet/core/install/linux)
3. Install [NodeJS](https://nodejs.org/en/download/package-manager)
4. Refer to each project readme.

## License

Apache License 2.0.
