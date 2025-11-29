/*******************************************************************
* Name: Casey Wormington
* Date: 11/29/2025
* Assignment: SDC320 Project
*
* Interface class IRecord - defines all methods that classes that
* implement this interface must implement.
*/
public interface IRecord
{
    decimal GetAmount();
    DateTime GetDate();
    string GetCategory();
    string GetDescription();
    decimal GetSignedAmount();
}