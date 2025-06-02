// Global using directives

global using System.Threading.RateLimiting;
global using FluentValidation;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.RateLimiting;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Primitives;
global using OpenTelemetry.Metrics;
global using OpenTelemetry.Resources;
global using OpenTelemetry.Trace;
global using PasswordBackendChallenge.Application.ApiKey;
global using PasswordBackendChallenge.Application.Services;
global using PasswordBackendChallenge.Shared.Dtos;
global using PasswordBackendChallenge.Shared.Metrics;
global using PasswordBackendChallenge.Shared.Models;