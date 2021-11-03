using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MtCoffee.Web.Models
{
    /// <summary>
    /// The intent is to create a standardized JSON payload that can be serialized
    /// and delivered to the front-end. Good for AJAX calls.
    /// </summary>
    public class JsonPayload<T>
    {
        public virtual bool IsSuccess { get { return StatusCode == ResponseStatusCode.SUCCESS; } }
        public virtual T Payload { get; set; }
        public virtual List<string> Errors { get; set; } = new List<string>();

        private string _StatusCode = null;
        public string StatusCode
        {
            get => this._StatusCode;
            set => _StatusCode = ResponseStatusCode.ValueOf(value) ?? ResponseStatusCode.UNKNOWN;
        }


        /// <summary>
        /// Initializes Errors to empty list.
        /// </summary>
        public JsonPayload()
        {
            this.Errors = new List<string>();
            this.StatusCode = ResponseStatusCode.SUCCESS;
        }

        /// <summary>
        /// Successful unless errors are specified later. Empty errors list.
        /// </summary>
        /// <param name="data"></param>
        public JsonPayload(T data)
        {
            this.Payload = data;
            this.Errors = new List<string>();
            this.StatusCode = ResponseStatusCode.SUCCESS;
        }

        /// <summary>
        /// Successful unless errors are specified later. Empty errors list.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="errors"></param>
        public JsonPayload(T data, List<string> errors)
        {
            this.Payload = data;
            this.Errors = errors;
            if (errors.Any()) this.StatusCode = ResponseStatusCode.NON_SUCCESS;
        }

        public JsonPayload(T data, string error)
        {
            this.Payload = data;
            this.StatusCode = ResponseStatusCode.NON_SUCCESS;
            this.Errors = new List<string>() { error };
        }


        /// <summary>
        /// Sets Status to INVALID_MODEL_STATE if any modelstate errors are detected.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonPayload<T> AddAnyErrorsFromModelState(ModelStateDictionary model)
        {
            if (model.IsValid == false)
            {
                var errs = model.Where(x => x.Value.Errors.Any()).SelectMany(x => x.Value.Errors.Select(z => z.ErrorMessage));
                if (errs.Any()) this.StatusCode = ResponseStatusCode.INVALID_MODEL_STATE;
                this.Errors.AddRange(errs);
            }

            return this;
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                MaxDepth = 10,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public JsonResult ToJsonResult(int? httpStatusCode = 200)
        {
            var rs = new JsonResult(this, new System.Text.Json.JsonSerializerOptions()
            {
                MaxDepth = 10,
                IgnoreNullValues = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            if (httpStatusCode != null)
            {
                rs.StatusCode = httpStatusCode.Value;
            }

            return rs;
        }

        //public HttpResponseMessage ToHttpResponseMessage(HttpRequestMessage request, HttpStatusCode statusCode, string reasonPhrase)
        //{
        //    var response = new HttpResponseMessage(statusCode)
        //    {
        //        Content = this.ToObjectContent(request),
        //        ReasonPhrase = reasonPhrase
        //    };

        //    return response;
        //}

        ///// <summary>
        ///// Useful because it is the same kind of object content that would normally be passed out of ApiController methods.
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public ObjectContent ToObjectContent(System.Net.Http.HttpRequestMessage request)
        //{
        //    var config = GlobalConfiguration.Configuration;
        //    IContentNegotiator negotiator = config.Services.GetContentNegotiator();
        //    ContentNegotiationResult result = negotiator.Negotiate(this.GetType(), request, config.Formatters);

        //    if (result != null)
        //    {
        //        return new ObjectContent(this.GetType(), this, result.Formatter, result.MediaType.MediaType); // value, media formatter, Mime type
        //    }

        //    return null;
        //}
    }
}
