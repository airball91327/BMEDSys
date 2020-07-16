//------------------------------------------------------------------------------
// <自動產生>
//     這段程式碼是由工具產生的。
//     //
//     變更此檔案可能會導致不正確的行為，而且若已重新產生
//     程式碼，則會遺失變更。
// </自動產生>
//------------------------------------------------------------------------------

namespace WebService
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://192.168.0.12/HcWebService/", ConfigurationName="WebService.ERPservicesSoap")]
    public interface ERPservicesSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.0.12/HcWebService/GetHolidays", ReplyAction="*")]
        System.Threading.Tasks.Task<WebService.GetHolidaysResponse> GetHolidaysAsync(WebService.GetHolidaysRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.0.12/HcWebService/PostLeaveData", ReplyAction="*")]
        System.Threading.Tasks.Task<WebService.PostLeaveDataResponse> PostLeaveDataAsync(WebService.PostLeaveDataRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.0.12/HcWebService/PostLeaveForAdd", ReplyAction="*")]
        System.Threading.Tasks.Task<WebService.PostLeaveForAddResponse> PostLeaveForAddAsync(WebService.PostLeaveForAddRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.0.12/HcWebService/PostRpKpCost", ReplyAction="*")]
        System.Threading.Tasks.Task<WebService.PostRpKpCostResponse> PostRpKpCostAsync(WebService.PostRpKpCostRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.0.12/HcWebService/GetStock", ReplyAction="*")]
        System.Threading.Tasks.Task<WebService.GetStockResponse> GetStockAsync(WebService.GetStockRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.0.12/HcWebService/GetEmployee", ReplyAction="*")]
        System.Threading.Tasks.Task<WebService.GetEmployeeResponse> GetEmployeeAsync(WebService.GetEmployeeRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.0.12/HcWebService/GetProduct", ReplyAction="*")]
        System.Threading.Tasks.Task<WebService.GetProductResponse> GetProductAsync(WebService.GetProductRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.0.12/HcWebService/PostRepStuff", ReplyAction="*")]
        System.Threading.Tasks.Task<WebService.PostRepStuffResponse> PostRepStuffAsync(WebService.PostRepStuffRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.0.12/HcWebService/GetCustomer", ReplyAction="*")]
        System.Threading.Tasks.Task<WebService.GetCustomerResponse> GetCustomerAsync(WebService.GetCustomerRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.0.12/HcWebService/GetVendor", ReplyAction="*")]
        System.Threading.Tasks.Task<WebService.GetVendorResponse> GetVendorAsync(WebService.GetVendorRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetHolidaysRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetHolidays", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.GetHolidaysRequestBody Body;
        
        public GetHolidaysRequest()
        {
        }
        
        public GetHolidaysRequest(WebService.GetHolidaysRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class GetHolidaysRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int year;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string empno;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string empname;
        
        public GetHolidaysRequestBody()
        {
        }
        
        public GetHolidaysRequestBody(int year, string empno, string empname)
        {
            this.year = year;
            this.empno = empno;
            this.empname = empname;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetHolidaysResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetHolidaysResponse", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.GetHolidaysResponseBody Body;
        
        public GetHolidaysResponse()
        {
        }
        
        public GetHolidaysResponse(WebService.GetHolidaysResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class GetHolidaysResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetHolidaysResult;
        
        public GetHolidaysResponseBody()
        {
        }
        
        public GetHolidaysResponseBody(string GetHolidaysResult)
        {
            this.GetHolidaysResult = GetHolidaysResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PostLeaveDataRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PostLeaveData", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.PostLeaveDataRequestBody Body;
        
        public PostLeaveDataRequest()
        {
        }
        
        public PostLeaveDataRequest(WebService.PostLeaveDataRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class PostLeaveDataRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string data;
        
        public PostLeaveDataRequestBody()
        {
        }
        
        public PostLeaveDataRequestBody(string data)
        {
            this.data = data;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PostLeaveDataResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PostLeaveDataResponse", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.PostLeaveDataResponseBody Body;
        
        public PostLeaveDataResponse()
        {
        }
        
        public PostLeaveDataResponse(WebService.PostLeaveDataResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class PostLeaveDataResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string PostLeaveDataResult;
        
        public PostLeaveDataResponseBody()
        {
        }
        
        public PostLeaveDataResponseBody(string PostLeaveDataResult)
        {
            this.PostLeaveDataResult = PostLeaveDataResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PostLeaveForAddRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PostLeaveForAdd", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.PostLeaveForAddRequestBody Body;
        
        public PostLeaveForAddRequest()
        {
        }
        
        public PostLeaveForAddRequest(WebService.PostLeaveForAddRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class PostLeaveForAddRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string data;
        
        public PostLeaveForAddRequestBody()
        {
        }
        
        public PostLeaveForAddRequestBody(string data)
        {
            this.data = data;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PostLeaveForAddResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PostLeaveForAddResponse", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.PostLeaveForAddResponseBody Body;
        
        public PostLeaveForAddResponse()
        {
        }
        
        public PostLeaveForAddResponse(WebService.PostLeaveForAddResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class PostLeaveForAddResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string PostLeaveForAddResult;
        
        public PostLeaveForAddResponseBody()
        {
        }
        
        public PostLeaveForAddResponseBody(string PostLeaveForAddResult)
        {
            this.PostLeaveForAddResult = PostLeaveForAddResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PostRpKpCostRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PostRpKpCost", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.PostRpKpCostRequestBody Body;
        
        public PostRpKpCostRequest()
        {
        }
        
        public PostRpKpCostRequest(WebService.PostRpKpCostRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class PostRpKpCostRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string mf;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string tf;
        
        public PostRpKpCostRequestBody()
        {
        }
        
        public PostRpKpCostRequestBody(string mf, string tf)
        {
            this.mf = mf;
            this.tf = tf;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PostRpKpCostResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PostRpKpCostResponse", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.PostRpKpCostResponseBody Body;
        
        public PostRpKpCostResponse()
        {
        }
        
        public PostRpKpCostResponse(WebService.PostRpKpCostResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class PostRpKpCostResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string PostRpKpCostResult;
        
        public PostRpKpCostResponseBody()
        {
        }
        
        public PostRpKpCostResponseBody(string PostRpKpCostResult)
        {
            this.PostRpKpCostResult = PostRpKpCostResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetStockRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetStock", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.GetStockRequestBody Body;
        
        public GetStockRequest()
        {
        }
        
        public GetStockRequest(WebService.GetStockRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class GetStockRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string pno;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string wno;
        
        public GetStockRequestBody()
        {
        }
        
        public GetStockRequestBody(string pno, string wno)
        {
            this.pno = pno;
            this.wno = wno;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetStockResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetStockResponse", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.GetStockResponseBody Body;
        
        public GetStockResponse()
        {
        }
        
        public GetStockResponse(WebService.GetStockResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class GetStockResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetStockResult;
        
        public GetStockResponseBody()
        {
        }
        
        public GetStockResponseBody(string GetStockResult)
        {
            this.GetStockResult = GetStockResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetEmployeeRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetEmployee", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.GetEmployeeRequestBody Body;
        
        public GetEmployeeRequest()
        {
        }
        
        public GetEmployeeRequest(WebService.GetEmployeeRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class GetEmployeeRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string eno;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string ename;
        
        public GetEmployeeRequestBody()
        {
        }
        
        public GetEmployeeRequestBody(string eno, string ename)
        {
            this.eno = eno;
            this.ename = ename;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetEmployeeResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetEmployeeResponse", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.GetEmployeeResponseBody Body;
        
        public GetEmployeeResponse()
        {
        }
        
        public GetEmployeeResponse(WebService.GetEmployeeResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class GetEmployeeResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetEmployeeResult;
        
        public GetEmployeeResponseBody()
        {
        }
        
        public GetEmployeeResponseBody(string GetEmployeeResult)
        {
            this.GetEmployeeResult = GetEmployeeResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetProductRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetProduct", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.GetProductRequestBody Body;
        
        public GetProductRequest()
        {
        }
        
        public GetProductRequest(WebService.GetProductRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class GetProductRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string pno;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string pname;
        
        public GetProductRequestBody()
        {
        }
        
        public GetProductRequestBody(string pno, string pname)
        {
            this.pno = pno;
            this.pname = pname;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetProductResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetProductResponse", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.GetProductResponseBody Body;
        
        public GetProductResponse()
        {
        }
        
        public GetProductResponse(WebService.GetProductResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class GetProductResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetProductResult;
        
        public GetProductResponseBody()
        {
        }
        
        public GetProductResponseBody(string GetProductResult)
        {
            this.GetProductResult = GetProductResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PostRepStuffRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PostRepStuff", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.PostRepStuffRequestBody Body;
        
        public PostRepStuffRequest()
        {
        }
        
        public PostRepStuffRequest(WebService.PostRepStuffRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class PostRepStuffRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string mf;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string tf;
        
        public PostRepStuffRequestBody()
        {
        }
        
        public PostRepStuffRequestBody(string mf, string tf)
        {
            this.mf = mf;
            this.tf = tf;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PostRepStuffResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PostRepStuffResponse", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.PostRepStuffResponseBody Body;
        
        public PostRepStuffResponse()
        {
        }
        
        public PostRepStuffResponse(WebService.PostRepStuffResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class PostRepStuffResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string PostRepStuffResult;
        
        public PostRepStuffResponseBody()
        {
        }
        
        public PostRepStuffResponseBody(string PostRepStuffResult)
        {
            this.PostRepStuffResult = PostRepStuffResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetCustomerRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetCustomer", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.GetCustomerRequestBody Body;
        
        public GetCustomerRequest()
        {
        }
        
        public GetCustomerRequest(WebService.GetCustomerRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class GetCustomerRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string uno;
        
        public GetCustomerRequestBody()
        {
        }
        
        public GetCustomerRequestBody(string uno)
        {
            this.uno = uno;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetCustomerResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetCustomerResponse", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.GetCustomerResponseBody Body;
        
        public GetCustomerResponse()
        {
        }
        
        public GetCustomerResponse(WebService.GetCustomerResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class GetCustomerResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetCustomerResult;
        
        public GetCustomerResponseBody()
        {
        }
        
        public GetCustomerResponseBody(string GetCustomerResult)
        {
            this.GetCustomerResult = GetCustomerResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetVendorRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetVendor", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.GetVendorRequestBody Body;
        
        public GetVendorRequest()
        {
        }
        
        public GetVendorRequest(WebService.GetVendorRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class GetVendorRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string uno;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string vname;
        
        public GetVendorRequestBody()
        {
        }
        
        public GetVendorRequestBody(string uno, string vname)
        {
            this.uno = uno;
            this.vname = vname;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetVendorResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetVendorResponse", Namespace="http://192.168.0.12/HcWebService/", Order=0)]
        public WebService.GetVendorResponseBody Body;
        
        public GetVendorResponse()
        {
        }
        
        public GetVendorResponse(WebService.GetVendorResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.0.12/HcWebService/")]
    public partial class GetVendorResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetVendorResult;
        
        public GetVendorResponseBody()
        {
        }
        
        public GetVendorResponseBody(string GetVendorResult)
        {
            this.GetVendorResult = GetVendorResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public interface ERPservicesSoapChannel : WebService.ERPservicesSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public partial class ERPservicesSoapClient : System.ServiceModel.ClientBase<WebService.ERPservicesSoap>, WebService.ERPservicesSoap
    {
        
    /// <summary>
    /// 實作此部分方法來設定服務端點。
    /// </summary>
    /// <param name="serviceEndpoint">要設定的端點</param>
    /// <param name="clientCredentials">用戶端認證</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public ERPservicesSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(ERPservicesSoapClient.GetBindingForEndpoint(endpointConfiguration), ERPservicesSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ERPservicesSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(ERPservicesSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ERPservicesSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(ERPservicesSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ERPservicesSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WebService.GetHolidaysResponse> WebService.ERPservicesSoap.GetHolidaysAsync(WebService.GetHolidaysRequest request)
        {
            return base.Channel.GetHolidaysAsync(request);
        }
        
        public System.Threading.Tasks.Task<WebService.GetHolidaysResponse> GetHolidaysAsync(int year, string empno, string empname)
        {
            WebService.GetHolidaysRequest inValue = new WebService.GetHolidaysRequest();
            inValue.Body = new WebService.GetHolidaysRequestBody();
            inValue.Body.year = year;
            inValue.Body.empno = empno;
            inValue.Body.empname = empname;
            return ((WebService.ERPservicesSoap)(this)).GetHolidaysAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WebService.PostLeaveDataResponse> WebService.ERPservicesSoap.PostLeaveDataAsync(WebService.PostLeaveDataRequest request)
        {
            return base.Channel.PostLeaveDataAsync(request);
        }
        
        public System.Threading.Tasks.Task<WebService.PostLeaveDataResponse> PostLeaveDataAsync(string data)
        {
            WebService.PostLeaveDataRequest inValue = new WebService.PostLeaveDataRequest();
            inValue.Body = new WebService.PostLeaveDataRequestBody();
            inValue.Body.data = data;
            return ((WebService.ERPservicesSoap)(this)).PostLeaveDataAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WebService.PostLeaveForAddResponse> WebService.ERPservicesSoap.PostLeaveForAddAsync(WebService.PostLeaveForAddRequest request)
        {
            return base.Channel.PostLeaveForAddAsync(request);
        }
        
        public System.Threading.Tasks.Task<WebService.PostLeaveForAddResponse> PostLeaveForAddAsync(string data)
        {
            WebService.PostLeaveForAddRequest inValue = new WebService.PostLeaveForAddRequest();
            inValue.Body = new WebService.PostLeaveForAddRequestBody();
            inValue.Body.data = data;
            return ((WebService.ERPservicesSoap)(this)).PostLeaveForAddAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WebService.PostRpKpCostResponse> WebService.ERPservicesSoap.PostRpKpCostAsync(WebService.PostRpKpCostRequest request)
        {
            return base.Channel.PostRpKpCostAsync(request);
        }
        
        public System.Threading.Tasks.Task<WebService.PostRpKpCostResponse> PostRpKpCostAsync(string mf, string tf)
        {
            WebService.PostRpKpCostRequest inValue = new WebService.PostRpKpCostRequest();
            inValue.Body = new WebService.PostRpKpCostRequestBody();
            inValue.Body.mf = mf;
            inValue.Body.tf = tf;
            return ((WebService.ERPservicesSoap)(this)).PostRpKpCostAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WebService.GetStockResponse> WebService.ERPservicesSoap.GetStockAsync(WebService.GetStockRequest request)
        {
            return base.Channel.GetStockAsync(request);
        }
        
        public System.Threading.Tasks.Task<WebService.GetStockResponse> GetStockAsync(string pno, string wno)
        {
            WebService.GetStockRequest inValue = new WebService.GetStockRequest();
            inValue.Body = new WebService.GetStockRequestBody();
            inValue.Body.pno = pno;
            inValue.Body.wno = wno;
            return ((WebService.ERPservicesSoap)(this)).GetStockAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WebService.GetEmployeeResponse> WebService.ERPservicesSoap.GetEmployeeAsync(WebService.GetEmployeeRequest request)
        {
            return base.Channel.GetEmployeeAsync(request);
        }
        
        public System.Threading.Tasks.Task<WebService.GetEmployeeResponse> GetEmployeeAsync(string eno, string ename)
        {
            WebService.GetEmployeeRequest inValue = new WebService.GetEmployeeRequest();
            inValue.Body = new WebService.GetEmployeeRequestBody();
            inValue.Body.eno = eno;
            inValue.Body.ename = ename;
            return ((WebService.ERPservicesSoap)(this)).GetEmployeeAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WebService.GetProductResponse> WebService.ERPservicesSoap.GetProductAsync(WebService.GetProductRequest request)
        {
            return base.Channel.GetProductAsync(request);
        }
        
        public System.Threading.Tasks.Task<WebService.GetProductResponse> GetProductAsync(string pno, string pname)
        {
            WebService.GetProductRequest inValue = new WebService.GetProductRequest();
            inValue.Body = new WebService.GetProductRequestBody();
            inValue.Body.pno = pno;
            inValue.Body.pname = pname;
            return ((WebService.ERPservicesSoap)(this)).GetProductAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WebService.PostRepStuffResponse> WebService.ERPservicesSoap.PostRepStuffAsync(WebService.PostRepStuffRequest request)
        {
            return base.Channel.PostRepStuffAsync(request);
        }
        
        public System.Threading.Tasks.Task<WebService.PostRepStuffResponse> PostRepStuffAsync(string mf, string tf)
        {
            WebService.PostRepStuffRequest inValue = new WebService.PostRepStuffRequest();
            inValue.Body = new WebService.PostRepStuffRequestBody();
            inValue.Body.mf = mf;
            inValue.Body.tf = tf;
            return ((WebService.ERPservicesSoap)(this)).PostRepStuffAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WebService.GetCustomerResponse> WebService.ERPservicesSoap.GetCustomerAsync(WebService.GetCustomerRequest request)
        {
            return base.Channel.GetCustomerAsync(request);
        }
        
        public System.Threading.Tasks.Task<WebService.GetCustomerResponse> GetCustomerAsync(string uno)
        {
            WebService.GetCustomerRequest inValue = new WebService.GetCustomerRequest();
            inValue.Body = new WebService.GetCustomerRequestBody();
            inValue.Body.uno = uno;
            return ((WebService.ERPservicesSoap)(this)).GetCustomerAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WebService.GetVendorResponse> WebService.ERPservicesSoap.GetVendorAsync(WebService.GetVendorRequest request)
        {
            return base.Channel.GetVendorAsync(request);
        }
        
        public System.Threading.Tasks.Task<WebService.GetVendorResponse> GetVendorAsync(string uno, string vname)
        {
            WebService.GetVendorRequest inValue = new WebService.GetVendorRequest();
            inValue.Body = new WebService.GetVendorRequestBody();
            inValue.Body.uno = uno;
            inValue.Body.vname = vname;
            return ((WebService.ERPservicesSoap)(this)).GetVendorAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.ERPservicesSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.ERPservicesSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("找不到名為 \'{0}\' 的端點。", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.ERPservicesSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://122.117.19.77/HcWebService/ERPservices.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.ERPservicesSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://122.117.19.77/HcWebService/ERPservices.asmx");
            }
            throw new System.InvalidOperationException(string.Format("找不到名為 \'{0}\' 的端點。", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            ERPservicesSoap,
            
            ERPservicesSoap12,
        }
    }
}
