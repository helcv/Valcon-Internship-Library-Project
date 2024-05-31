using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValconLibrary.Helpers;

namespace ValconLibrary.Test.Mocks
{
    public static class MockMapper
    {
        public static IMapper GetMapperConfig()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });

            return configuration.CreateMapper();
        }
    }
}
