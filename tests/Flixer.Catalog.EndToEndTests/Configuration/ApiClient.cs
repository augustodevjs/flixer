﻿using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Flixer.Catalog.Infra.Messaging.JsonPolicies;

namespace Flixer.Catalog.EndToEndTests.Configuration;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _defaultJsonSerializerOptions;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _defaultJsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = new JsonSnakeCasePolicy(),
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(
        string route, 
        object payload
    ) where TOutput : class
    {
        var response = await _httpClient.PostAsync(
            route,
            new StringContent(
                JsonSerializer.Serialize(payload, _defaultJsonSerializerOptions),
                Encoding.UTF8,
                "application/json"
            )
        );

        var output = await GetContentObject<TOutput>(response);

        return (response, output);
    }
    
    public async Task<(HttpResponseMessage?, TOutput?)> Put<TOutput>(
        string route, 
        object payload
    ) where TOutput : class
    {
        var response = await _httpClient.PutAsync(
            route,
            new StringContent(
                JsonSerializer.Serialize(payload, _defaultJsonSerializerOptions),
                Encoding.UTF8,
                "application/json"
            )
        );

        var output = await GetContentObject<TOutput>(response);

        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Get<TOutput>(
        string route, 
        object? queryStringParametersObject = null
    ) where TOutput : class
    {
        var url = PrepareGetRoute(route, queryStringParametersObject);

        var response = await _httpClient.GetAsync(url);
        var output = await GetContentObject<TOutput>(response);

        return (response, output);
    }
    
    public async Task<(HttpResponseMessage?, TOutput?)> Delete<TOutput>
        (string route) where TOutput : class
    {
        var response = await _httpClient.DeleteAsync(route);
        var output = await GetContentObject<TOutput>(response);

        return (response, output);
    }

    private async Task<TOutput?> GetContentObject<TOutput>
        (HttpResponseMessage response) where TOutput : class
    {
        var outputString = await response.Content.ReadAsStringAsync();

        TOutput? output = null;

        if (!string.IsNullOrWhiteSpace(outputString))
            output = JsonSerializer.Deserialize<TOutput>(
                outputString,
                _defaultJsonSerializerOptions
            );

        return output;
    }

    private string PrepareGetRoute(string route, object? queryStringParametersObject)
    {
        if (queryStringParametersObject is null)
            return route;

        var parametersJson = JsonSerializer.Serialize(
            queryStringParametersObject, 
            _defaultJsonSerializerOptions
        );

        var parametersDictionary = Newtonsoft.Json.JsonConvert
            .DeserializeObject<Dictionary<string, string>>(parametersJson);

        return QueryHelpers.AddQueryString(route, parametersDictionary!);
    }
}
