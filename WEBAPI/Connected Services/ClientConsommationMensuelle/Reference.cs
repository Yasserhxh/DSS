﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientConsommationMensuelle
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", ConfigurationName="ClientConsommationMensuelle.ZSD_MCSI_S805_WSR")]
    public interface ZSD_MCSI_S805_WSR
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:sap-com:document:sap:rfc:functions:ZSD_MCSI_S805_WSR:ZSD_MCSI_S805Request", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<ClientConsommationMensuelle.ZSD_MCSI_S805Response1> ZSD_MCSI_S805Async(ClientConsommationMensuelle.ZSD_MCSI_S805Request request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZSD_MCSI_S805
    {
        
        private ZSTCUST_SALES[] t_CUST_SALESField;
        
        private ZSTKUNAG[] zKUNAGField;
        
        private ZSTSPTAG[] zSPTAGField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZSTCUST_SALES[] T_CUST_SALES
        {
            get
            {
                return this.t_CUST_SALESField;
            }
            set
            {
                this.t_CUST_SALESField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZSTKUNAG[] ZKUNAG
        {
            get
            {
                return this.zKUNAGField;
            }
            set
            {
                this.zKUNAGField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZSTSPTAG[] ZSPTAG
        {
            get
            {
                return this.zSPTAGField;
            }
            set
            {
                this.zSPTAGField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZSTCUST_SALES
    {
        
        private string vKORGField;
        
        private string vSTELField;
        
        private string pKUNAGField;
        
        private string mATNRField;
        
        private string pERIODField;
        
        private string pERYEARField;
        
        private decimal fKIMGField;
        
        private string u_FKIMGField;
        
        private decimal nETWRField;
        
        private string u_NETWRField;
        
        private decimal wAVWRField;
        
        private string u_WAVWRField;
        
        private decimal nTGEWField;
        
        private string u_NTGEWField;
        
        private decimal kZWI1Field;
        
        private string u_KZWI1Field;
        
        private decimal kZWI2Field;
        
        private string u_KZWI2Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string VKORG
        {
            get
            {
                return this.vKORGField;
            }
            set
            {
                this.vKORGField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string VSTEL
        {
            get
            {
                return this.vSTELField;
            }
            set
            {
                this.vSTELField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string PKUNAG
        {
            get
            {
                return this.pKUNAGField;
            }
            set
            {
                this.pKUNAGField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string MATNR
        {
            get
            {
                return this.mATNRField;
            }
            set
            {
                this.mATNRField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string PERIOD
        {
            get
            {
                return this.pERIODField;
            }
            set
            {
                this.pERIODField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string PERYEAR
        {
            get
            {
                return this.pERYEARField;
            }
            set
            {
                this.pERYEARField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public decimal FKIMG
        {
            get
            {
                return this.fKIMGField;
            }
            set
            {
                this.fKIMGField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string U_FKIMG
        {
            get
            {
                return this.u_FKIMGField;
            }
            set
            {
                this.u_FKIMGField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public decimal NETWR
        {
            get
            {
                return this.nETWRField;
            }
            set
            {
                this.nETWRField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string U_NETWR
        {
            get
            {
                return this.u_NETWRField;
            }
            set
            {
                this.u_NETWRField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=10)]
        public decimal WAVWR
        {
            get
            {
                return this.wAVWRField;
            }
            set
            {
                this.wAVWRField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=11)]
        public string U_WAVWR
        {
            get
            {
                return this.u_WAVWRField;
            }
            set
            {
                this.u_WAVWRField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=12)]
        public decimal NTGEW
        {
            get
            {
                return this.nTGEWField;
            }
            set
            {
                this.nTGEWField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=13)]
        public string U_NTGEW
        {
            get
            {
                return this.u_NTGEWField;
            }
            set
            {
                this.u_NTGEWField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=14)]
        public decimal KZWI1
        {
            get
            {
                return this.kZWI1Field;
            }
            set
            {
                this.kZWI1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=15)]
        public string U_KZWI1
        {
            get
            {
                return this.u_KZWI1Field;
            }
            set
            {
                this.u_KZWI1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=16)]
        public decimal KZWI2
        {
            get
            {
                return this.kZWI2Field;
            }
            set
            {
                this.kZWI2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=17)]
        public string U_KZWI2
        {
            get
            {
                return this.u_KZWI2Field;
            }
            set
            {
                this.u_KZWI2Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZSTSPTAG
    {
        
        private string sIGNField;
        
        private string oPTIONField;
        
        private string lOWField;
        
        private string hIGHField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string SIGN
        {
            get
            {
                return this.sIGNField;
            }
            set
            {
                this.sIGNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string OPTION
        {
            get
            {
                return this.oPTIONField;
            }
            set
            {
                this.oPTIONField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string LOW
        {
            get
            {
                return this.lOWField;
            }
            set
            {
                this.lOWField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string HIGH
        {
            get
            {
                return this.hIGHField;
            }
            set
            {
                this.hIGHField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZSTKUNAG
    {
        
        private string sIGNField;
        
        private string oPTIONField;
        
        private string lOWField;
        
        private string hIGHField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string SIGN
        {
            get
            {
                return this.sIGNField;
            }
            set
            {
                this.sIGNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string OPTION
        {
            get
            {
                return this.oPTIONField;
            }
            set
            {
                this.oPTIONField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string LOW
        {
            get
            {
                return this.lOWField;
            }
            set
            {
                this.lOWField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string HIGH
        {
            get
            {
                return this.hIGHField;
            }
            set
            {
                this.hIGHField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZSD_MCSI_S805Response
    {
        
        private ZSTCUST_SALES[] t_CUST_SALESField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZSTCUST_SALES[] T_CUST_SALES
        {
            get
            {
                return this.t_CUST_SALESField;
            }
            set
            {
                this.t_CUST_SALESField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ZSD_MCSI_S805Request
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        public ClientConsommationMensuelle.ZSD_MCSI_S805 ZSD_MCSI_S805;
        
        public ZSD_MCSI_S805Request()
        {
        }
        
        public ZSD_MCSI_S805Request(ClientConsommationMensuelle.ZSD_MCSI_S805 ZSD_MCSI_S805)
        {
            this.ZSD_MCSI_S805 = ZSD_MCSI_S805;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ZSD_MCSI_S805Response1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        public ClientConsommationMensuelle.ZSD_MCSI_S805Response ZSD_MCSI_S805Response;
        
        public ZSD_MCSI_S805Response1()
        {
        }
        
        public ZSD_MCSI_S805Response1(ClientConsommationMensuelle.ZSD_MCSI_S805Response ZSD_MCSI_S805Response)
        {
            this.ZSD_MCSI_S805Response = ZSD_MCSI_S805Response;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public interface ZSD_MCSI_S805_WSRChannel : ClientConsommationMensuelle.ZSD_MCSI_S805_WSR, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public partial class ZSD_MCSI_S805_WSRClient : System.ServiceModel.ClientBase<ClientConsommationMensuelle.ZSD_MCSI_S805_WSR>, ClientConsommationMensuelle.ZSD_MCSI_S805_WSR
    {
        
        public ZSD_MCSI_S805_WSRClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ClientConsommationMensuelle.ZSD_MCSI_S805Response1> ClientConsommationMensuelle.ZSD_MCSI_S805_WSR.ZSD_MCSI_S805Async(ClientConsommationMensuelle.ZSD_MCSI_S805Request request)
        {
            return base.Channel.ZSD_MCSI_S805Async(request);
        }
        
        public System.Threading.Tasks.Task<ClientConsommationMensuelle.ZSD_MCSI_S805Response1> ZSD_MCSI_S805Async(ClientConsommationMensuelle.ZSD_MCSI_S805 ZSD_MCSI_S805)
        {
            ClientConsommationMensuelle.ZSD_MCSI_S805Request inValue = new ClientConsommationMensuelle.ZSD_MCSI_S805Request();
            inValue.ZSD_MCSI_S805 = ZSD_MCSI_S805;
            return ((ClientConsommationMensuelle.ZSD_MCSI_S805_WSR)(this)).ZSD_MCSI_S805Async(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
    }
}
