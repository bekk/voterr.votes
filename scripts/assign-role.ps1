$resourceGroupName = "Voterr"
$accountName = "voterr-db"
$readOnlyRoleDefinitionId = "/subscriptions/9539bc24-8692-4fe2-871e-3733e84b1b73/resourceGroups/Voterr/providers/Microsoft.DocumentDB/databaseAccounts/voterr-db/sqlRoleDefinitions/6ce67d5e-475b-4324-bccf-63ee18beee30" 
$principalId = "7ef0e183-649b-48a2-bd01-70d83195de68"
New-AzCosmosDBSqlRoleAssignment -AccountName $accountName `
    -ResourceGroupName $resourceGroupName `
    -RoleDefinitionId $readOnlyRoleDefinitionId `
    -Scope "/" `
    -PrincipalId $principalId    