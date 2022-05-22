﻿using EmailService.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.WebApi.Services
{
public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}
}
