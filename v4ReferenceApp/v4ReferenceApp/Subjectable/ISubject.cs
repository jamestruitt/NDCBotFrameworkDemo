using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using v4ReferenceApp.Context;

namespace v4ReferenceApp.Subjectable
{
    public interface ISubject
    {

        string SubjectName { get; set; }

        Task<bool> StartSubject(V4ReferenceContext context);


        Task<bool> ContinueSubject(V4ReferenceContext context);

        ISubject ParentSubject { get; set; }

    }
}
