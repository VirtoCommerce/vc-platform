/**********************************************************************/
/* Install.SQL                                                        */
/* Creates a login and makes the user a member of db roles            */
/*                                                                    */
/*           Modifications for SQL AZURE  - ON USER DB                */
/**********************************************************************/



-- Create database user and map to login
-- and add user to the datareader, datawriter, ddladmin and securityadmin roles
--

    CREATE USER PlaceHolderForUser FOR LOGIN PlaceHolderForUser;
	GO
    EXEC sp_addrolemember 'db_owner', PlaceHolderForUser;
	GO