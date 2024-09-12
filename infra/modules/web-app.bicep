param planName string
param webAppName string
param location string = resourceGroup().location

resource plan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: planName
  location: 'eastus'
  properties: {
    reserved: false // Windows
  }
  sku: {
    name: 'F1'
  }
}

resource webApp 'Microsoft.Web/sites@2020-06-01' = {
  name: webAppName
  location: location
  properties: {
    serverFarmId: plan.id
  }
}
