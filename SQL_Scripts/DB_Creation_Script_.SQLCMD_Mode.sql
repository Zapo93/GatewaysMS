:setvar path "C:\Users\Zapo\Desktop\GatewaysMS\SQL_Scripts"

:r $(path)\CreateScripts\CreateMS_GatewaysDatabase.sql

:r $(path)\CreateScripts\CreateGatewayTable.sql
:r $(path)\CreateScripts\CreateUniqueIndexGatewaySerialNumber.sql

:r $(path)\CreateScripts\CreateDeviceTable.sql
:r $(path)\CreateScripts\CreateSequenceDeviceUniqueId.sql

:r $(path)\CreateScripts\CreateDeviceStatusTable.sql
:r $(path)\CreateScripts\CreateUniqueIndexDeviceId.sql


:r $(path)\ProceduresAndQueries\AddGateway_StoredProcedure.sql

:r $(path)\ProceduresAndQueries\DeleteGateway_StoredProcedure.sql

:r $(path)\ProceduresAndQueries\AddDevice_StoredProcedure.sql

:r $(path)\ProceduresAndQueries\DeleteDevice_StoredProcedure.sql

:r $(path)\ProceduresAndQueries\GetAllGateways_StoredProcedure.sql

:r $(path)\ProceduresAndQueries\GetGatewayBySerialNumber_StoredProcedure.sql

:r $(path)\ProceduresAndQueries\UpdateDeviceStatus.sql


:r $(path)\ProceduresAndQueries\InsertExampleData.sql