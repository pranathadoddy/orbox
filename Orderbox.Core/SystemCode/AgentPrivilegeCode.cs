using Framework.Core;
using Orderbox.Core.Resources.Common;
using System;
using System.Collections.Generic;

namespace Orderbox.Core.SystemCode
{
    public class AgentPrivilegeCode : SystemCodeBase<string>
    {
        #region Fields

        private static readonly Lazy<AgentPrivilegeCode> Lazy =
            new Lazy<AgentPrivilegeCode>(() => new AgentPrivilegeCode());

        #endregion

        #region Constructors

        private AgentPrivilegeCode()
        {
            this.CodeList = new List<SystemCodeModel<string>>
            {
                new SystemCodeModel<string>(CoreConstant.AgentPrivilegeOption.Director, AgentResource.Director),
                new SystemCodeModel<string>(CoreConstant.AgentPrivilegeOption.Manager, AgentResource.Manager)                
            };
        }

        #endregion

        #region Properties

        public static AgentPrivilegeCode Item
        {
            get { return Lazy.Value; }
        }

        #endregion
    }
}
