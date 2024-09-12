targetScope = 'subscription'

param location string
param resourceGroupName string
param webAppName string
param webAppPlan string

resource rg 'Microsoft.Resources/resourceGroups@2020-06-01' = {
  name: '${resourceGroupName}-${location}'
  location: 'eastus'
}

module webApp './modules/web-app.bicep' = {
  scope: rg
  name: 'webApp-deployment'
  params: {
    planName: webAppPlan
    webAppName: webAppName
  }
}
