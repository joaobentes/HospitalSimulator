# Base OS Layer: Latest DotNet Docker for Linux
FROM microsoft/dotnet:latest

# Prepare folder and copy project folders and files
RUN mkdir /opt/HospitalSimulator
WORKDIR /opt/HospitalSimulator
COPY . .

# Build required libraries
RUN dotnet restore

# Expose server port
EXPOSE 5000

# Run .NET Core Server
CMD ["/usr/bin/dotnet", "run"]