using JA.FinancePark.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace JA.FinancePark.Core.Entities
{
    public class BaseEntity
    {
        public override string ToString()
        {
            string result = TypeHelper.GetStringRepresentation(this);
            return result;
        }
    }
}
