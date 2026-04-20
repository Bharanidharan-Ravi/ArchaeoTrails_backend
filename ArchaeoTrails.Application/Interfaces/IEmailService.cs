using ArchaeoTrails.Application.Features.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchaeoTrails.Application.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendContactEmailAsync(ContactRequest request);
    }
}
