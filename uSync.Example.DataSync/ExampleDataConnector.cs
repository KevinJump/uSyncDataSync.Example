using System;
using System.Collections.Generic;

using Umbraco.Core;
using Umbraco.Core.Deploy;

namespace uSync.Example.DataSync
{
    /// <summary>
    ///  A service connector is an umbraco.deploy thing but it is the only way to add a custom 
    ///  Udi type to umbraco, so you don't have to impliment any of the class here, but you 
    ///  need to define your udi in the UdiDefinition attribute of a class that inherits IServiceConnector
    /// </summary>
    [UdiDefinition(uSyncExampleDataSync.EntityType, UdiType.GuidUdi)]
    public class ExampleDataConnector : IServiceConnector
    {
        public bool Compare(IArtifact art1, IArtifact art2, ICollection<Difference> differences = null)
        {
            throw new NotImplementedException();
        }

        public void Explode(UdiRange range, List<Udi> udis)
        {
            throw new NotImplementedException();
        }

        public IArtifact GetArtifact(Udi udi)
        {
            throw new NotImplementedException();
        }

        public IArtifact GetArtifact(object entity)
        {
            throw new NotImplementedException();
        }

        public NamedUdiRange GetRange(Udi udi, string selector)
        {
            throw new NotImplementedException();
        }

        public NamedUdiRange GetRange(string entityType, string sid, string selector)
        {
            throw new NotImplementedException();
        }

        public void Process(ArtifactDeployState dart, IDeployContext context, int pass)
        {
            throw new NotImplementedException();
        }

        public ArtifactDeployState ProcessInit(IArtifact art, IDeployContext context)
        {
            throw new NotImplementedException();
        }
    }
}
