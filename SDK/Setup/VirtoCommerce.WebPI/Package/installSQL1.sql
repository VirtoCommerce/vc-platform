/**********************************************************************/
/* Install.SQL                                                        */
/* Creates a login and makes the user a member of db roles            */
/*                                                                    */
/*           Modifications for SQL AZURE  - ON MASTER                 */
/**********************************************************************/


-- Add here a test to be sure the login does not exist
-- Create login

CREATE LOGIN PlaceHolderForUser WITH PASSWORD = 'PlaceHolderForPassword'
