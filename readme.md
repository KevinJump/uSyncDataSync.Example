# uSync Extension Examples

This repo contains example code that shows how you can implement your own uSync Handler and Serializer to transport your own custom data between umbraco installations via uSync. 

## Stucture

### uSyncExamples.Site
Umbraco WebSite

### uSyncExamples.Data
Sample property editor, backed with a Database driven picker. 

This example is super simple mainly so we can have some data to share between installations. 

### uSync.Example.DataSync
Custom uSync Handler/Serializer and Mapper that integrate the data layer into uSync elements. 

## Key elements
within the uSync.Example.DataSync project we have implemented examples of the elements needed to integrate with uSync/ContentEdition and Complete. 

## Serializer 
Responsible for reading/writing elements to and from XML

## Handler 
Responsible for reading/Writing the XML to disk, you would also link into any Saving/Deleting events you may have within your custom data editors. 

## Mapper
If your data is stored within properties of content or media items, a mapper lets you change how that data is stored on disk  (e.g removal of Internal IDs etc)

### Dependency Checks
mappers are also responsible for calculating any additional dependencies that might be required for a property to be correctly setup on a target site.

in our example this is the database entry for the selected item and a linked document type. 


## Notes:
It is important that you have someway of identifying your custom items that can be generic between implementations. e.g an alias or unique-key (guid) value. without something to differentiate between items uSync cannot tell what is new and what is existing when it performs a sync between instances. 

you don't need both an alias and key - but uSync has been developed to work best with both - as if the value cannot be found by key, it will be searched for by alias. This allows us to sync previously un-synced systems, where the alias is the only matching element, and not rely solely on the key. 

but if all you have is a key/alias and you know your system is going to be in sync from teh start this is fine. 