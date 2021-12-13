//System Includes
global using System.Net;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Security.Claims;

//AspNetCore Includes
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.Extensions.Options;

//Identity Model Includes
global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;

//Dotnet Entity Framework Includes
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

//Automapper Includes
global using AutoMapper;
global using AutoMapper.QueryableExtensions;

//Cloudinary Includes
global using CloudinaryDotNet;
global using CloudinaryDotNet.Actions;

//API Includes
global using API.Data;
global using API.Entities;
global using API.Extensions;
global using API.Middleware;
global using API.SignalR;
global using API.Errors;
global using API.DTOs;
global using API.Interfaces;
global using API.Helpers;
global using API.Services;