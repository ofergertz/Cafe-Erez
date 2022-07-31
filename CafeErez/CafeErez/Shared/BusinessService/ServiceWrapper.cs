using CafeErez.Shared.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.BusinessService
{
    public class ServiceWrapper : IServiceWrapper
    {
        public ServiceWrapper()
        {
        }

        public List<string> Messages { get; set; } = new List<string>();

        public bool Succeeded { get; set; }

        public static IServiceWrapper Fail()
        {
            return new ServiceWrapper { Succeeded = false };
        }

        public static IServiceWrapper Fail(string message)
        {
            return new ServiceWrapper { Succeeded = false, Messages = new List<string> { message } };
        }

        public static IServiceWrapper Fail(List<string> messages)
        {
            return new ServiceWrapper { Succeeded = false, Messages = messages };
        }

        public static Task<IServiceWrapper> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public static Task<IServiceWrapper> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public static Task<IServiceWrapper> FailAsync(List<string> messages)
        {
            return Task.FromResult(Fail(messages));
        }

        public static IServiceWrapper Success()
        {
            return new ServiceWrapper { Succeeded = true };
        }

        public static IServiceWrapper Success(string message)
        {
            return new ServiceWrapper { Succeeded = true, Messages = new List<string> { message } };
        }

        public static Task<IServiceWrapper> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<IServiceWrapper> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }
    }

    public class ServiceWrapper<T> : ServiceWrapper, IServiceWrapper<T>
    {
        public ServiceWrapper()
        {
        }

        public T Data { get; set; }

        public new static ServiceWrapper<T> Fail()
        {
            return new ServiceWrapper<T> { Succeeded = false };
        }

        public new static ServiceWrapper<T> Fail(string message)
        {
            return new ServiceWrapper<T> { Succeeded = false, Messages = new List<string> { message } };
        }

        public new static ServiceWrapper<T> Fail(List<string> messages)
        {
            return new ServiceWrapper<T> { Succeeded = false, Messages = messages };
        }

        public new static Task<ServiceWrapper<T>> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public new static Task<ServiceWrapper<T>> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public new static Task<ServiceWrapper<T>> FailAsync(List<string> messages)
        {
            return Task.FromResult(Fail(messages));
        }

        public new static ServiceWrapper<T> Success()
        {
            return new ServiceWrapper<T> { Succeeded = true };
        }

        public new static ServiceWrapper<T> Success(string message)
        {
            return new ServiceWrapper<T> { Succeeded = true, Messages = new List<string> { message } };
        }

        public static ServiceWrapper<T> Success(T data)
        {
            return new ServiceWrapper<T> { Succeeded = true, Data = data };
        }

        public static ServiceWrapper<T> Success(T data, string message)
        {
            return new ServiceWrapper<T> { Succeeded = true, Data = data, Messages = new List<string> { message } };
        }

        public static ServiceWrapper<T> Success(T data, List<string> messages)
        {
            return new ServiceWrapper<T> { Succeeded = true, Data = data, Messages = messages };
        }

        public new static Task<ServiceWrapper<T>> SuccessAsync(Microsoft.AspNetCore.Identity.IdentityResult user)
        {
            return Task.FromResult(Success());
        }

        public new static Task<ServiceWrapper<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<ServiceWrapper<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<ServiceWrapper<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }
    }
}
