using System;

namespace DotNet.Template.Data.Model
{
    public interface IAudit
    {
        string UpdatedBy { get; set; }
        DateTime UpdateDate { get; set; }

        string CreatedBy { get; set; }
        DateTime CreateDate { get; set; }
    }
}
