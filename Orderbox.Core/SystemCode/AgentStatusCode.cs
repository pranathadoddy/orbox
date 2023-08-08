using Framework.Core;
using Orderbox.Core.Resources.Common;
using System;
using System.Collections.Generic;

namespace Orderbox.Core.SystemCode
{
    public class AgentStatusCode : SystemCodeBase<string>
    {
        #region Fields

        private static readonly Lazy<AgentStatusCode> Lazy =
            new Lazy<AgentStatusCode>(() => new AgentStatusCode());

        #endregion

        #region Constructors

        private AgentStatusCode()
        {
            this.CodeList = new List<SystemCodeModel<string>>
            {
                new SystemCodeModel<string>(CoreConstant.AgentStatus.Activating, AgentResource.Status_Activating),
                new SystemCodeModel<string>(CoreConstant.AgentStatus.Active, AgentResource.Status_Active),
            };
        }

        #endregion

        #region Properties

        public static AgentStatusCode Item
        {
            get { return Lazy.Value; }
        }

        #endregion
    }
}
