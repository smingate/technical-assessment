using Demo_API.Controllers;
using Demo_API.Iservice;
using Demo_API.Models;
using Demo_API.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Demo_API.Test
{
    public class DemoControllerTest
    {
        DemoController _controller;
        ITargetAssetService _targetAssetService;
        HttpClient _httpClient;

        public DemoControllerTest() 
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://06ba2c18-ac5b-4e14-988c-94f400643ebf.mock.pstmn.io/targetAsset")
            };
            _targetAssetService = new TargetAssetService(_httpClient);
            _controller = new DemoController(_targetAssetService);

        }

        [Fact]
        public async void GetAllTargetAssets()
        {
            //Arrange

            var targetAssetMock = createMockTargetAsset();

            var httpClient = new HttpClient(new HttpMessageHandlerMock(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(targetAssetMock), Encoding.UTF8, "application/json")
            }));

            httpClient.BaseAddress = new Uri("https://06ba2c18-ac5b-4e14-988c-94f400643ebf.mock.pstmn.io/targetAsset");
            _targetAssetService = new TargetAssetService(httpClient);
            _controller = new DemoController(_targetAssetService);

            //Act
            var result = await _controller.GetTargetAsset();
            var successResult = result as OkObjectResult;

            //Asset

            Assert.NotNull(successResult);
            Assert.IsType<List<TargetAsset>>(successResult.Value);
            Assert.Equal(200, successResult.StatusCode);
        }

        [Fact]
        public async void GetAllTargetAssetsWithZeroResults()
        {
            //Arrange

            var targetAssetMock = new List<TargetAsset>();

            var httpClient = new HttpClient(new HttpMessageHandlerMock(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(targetAssetMock), Encoding.UTF8, "application/json")
            }));

            httpClient.BaseAddress = new Uri("https://06ba2c18-ac5b-4e14-988c-94f400643ebf.mock.pstmn.io/targetAsset");
            _targetAssetService = new TargetAssetService(httpClient);
            _controller = new DemoController(_targetAssetService);

            //Act
            var result = await _controller.GetTargetAsset();
            var successResult = result as NotFoundResult;

            //Asset

            Assert.NotNull(successResult);
            Assert.Equal(404, successResult.StatusCode);
        }

        [Fact]
        public async void GetAllTargetAssetWithBadUrl()
        {
            //Arrange
            var targetAssetMock = createMockTargetAsset();

            var httpClient = new HttpClient(new HttpMessageHandlerMock(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
               // Content = new StringContent(JsonConvert.SerializeObject(targetAssetMock), Encoding.UTF8, "application/json")
            }));

            httpClient.BaseAddress = new Uri("https://06ba2c18-a8c-94f400643ebf.mock.pstmn.io/targetAs");
            _targetAssetService = new TargetAssetService(httpClient);
            _controller = new DemoController(_targetAssetService);


            //Act
            var result = await _controller.GetTargetAsset();
            var errorResult = result as ObjectResult;


            //Asset
            Assert.NotNull(errorResult);
            Assert.Equal(404, errorResult.StatusCode);
        }

        public List<TargetAsset> createMockTargetAsset()
        {
            return new List<TargetAsset> 
            { 
                new TargetAsset
                {
                    id = 1,
                    isStartable = true,
                    location = "Berlin",
                    owner = "jon.wayne@example.com",
                    createdBy = "christian.bale@example.com",
                    name = "SRVDEV01",
                    status = "Running",
                    cpu = 4,
                    ram = 6442450944,
                    parentId = 4
                },
                new TargetAsset
                {
                    id = 4,
                    isStartable = true,
                    location = "Zurich",
                    owner = "christian.bale@example.com",
                    createdBy = "christian.bale@example.com",
                    name = "SRVDEV01",
                    status = "Running",
                    cpu = 4,
                    ram = 6442450944,
                    parentId = 5
                },
                new TargetAsset
                {
                    id = 5,
                    isStartable = true,
                    location = "Paris",
                    owner = "peter.parker@example.com",
                    createdBy = "christian.bale@example.com",
                    name = "SRVDEV01",
                    status = "Running",
                    cpu = 4,
                    ram = 6442450944,
                    parentId = 2
                }
            };
        }

    }

    public class HttpMessageHandlerMock : HttpMessageHandler
    {
        private readonly HttpResponseMessage? _response;
        private readonly HttpStatusCode _code;

        public HttpMessageHandlerMock(HttpStatusCode code)
        {
            _code = code; ;
        }
        public HttpMessageHandlerMock(HttpResponseMessage response)
        {
            _response = response;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_response != null)
            {
                return Task.FromResult(_response);
            }

            return Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = _code,
            });
        }
    }

}