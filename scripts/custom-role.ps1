# prerequisite: Install-Module -Name Az.CosmosDB -RequiredVersion 1.2.0

$resourceGroupName = "Voterr"
$accountName = "voterr-db"
New-AzCosmosDBSqlRoleDefinition -AccountName $accountName `
    -ResourceGroupName $resourceGroupName `
    -Type CustomRole -RoleName VoterrReadWriteRole `
    -DataAction @( `
        'Microsoft.DocumentDB/databaseAccounts/readMetadata',
        'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/items/*', `
        'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/*') `
    -AssignableScope "/"