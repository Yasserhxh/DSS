﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//
//     Les changements apportés à ce fichier peuvent provoquer un comportement incorrect et seront perdus si
//     le code est regénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GetClientPartnerFS
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", ConfigurationName="GetClientPartnerFS.ZCUSTOMER_PARTNERFS_GET")]
    public interface ZCUSTOMER_PARTNERFS_GET
    {
        
        // CODEGEN : La génération du contrat de message depuis l'opération CUSTOMER_PARTNERFS_GET n'est ni RPC, ni encapsulée dans un document.
        [System.ServiceModel.OperationContractAttribute(Action="urn:sap-com:document:sap:rfc:functions:ZCUSTOMER_PARTNERFS_GET:CUSTOMER_PARTNERFS" +
            "_GETRequest", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        GetClientPartnerFS.CUSTOMER_PARTNERFS_GETResponse1 CUSTOMER_PARTNERFS_GET(GetClientPartnerFS.CUSTOMER_PARTNERFS_GETRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:sap-com:document:sap:rfc:functions:ZCUSTOMER_PARTNERFS_GET:CUSTOMER_PARTNERFS" +
            "_GETRequest", ReplyAction="*")]
        System.Threading.Tasks.Task<GetClientPartnerFS.CUSTOMER_PARTNERFS_GETResponse1> CUSTOMER_PARTNERFS_GETAsync(GetClientPartnerFS.CUSTOMER_PARTNERFS_GETRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class CUSTOMER_PARTNERFS_GET
    {
        
        private E1KNVPM[] eT_E1KNVPMField;
        
        private string iV_KUNNRField;
        
        private string iV_SPARTField;
        
        private string iV_VKORGField;
        
        private string iV_VTWEGField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public E1KNVPM[] ET_E1KNVPM
        {
            get
            {
                return this.eT_E1KNVPMField;
            }
            set
            {
                this.eT_E1KNVPMField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string IV_KUNNR
        {
            get
            {
                return this.iV_KUNNRField;
            }
            set
            {
                this.iV_KUNNRField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string IV_SPART
        {
            get
            {
                return this.iV_SPARTField;
            }
            set
            {
                this.iV_SPARTField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string IV_VKORG
        {
            get
            {
                return this.iV_VKORGField;
            }
            set
            {
                this.iV_VKORGField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string IV_VTWEG
        {
            get
            {
                return this.iV_VTWEGField;
            }
            set
            {
                this.iV_VTWEGField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class E1KNVPM
    {
        
        private string mSGFNField;
        
        private string pARVWField;
        
        private string kUNN2Field;
        
        private string dEFPAField;
        
        private string kNREFField;
        
        private string pARZAField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string MSGFN
        {
            get
            {
                return this.mSGFNField;
            }
            set
            {
                this.mSGFNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string PARVW
        {
            get
            {
                return this.pARVWField;
            }
            set
            {
                this.pARVWField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string KUNN2
        {
            get
            {
                return this.kUNN2Field;
            }
            set
            {
                this.kUNN2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string DEFPA
        {
            get
            {
                return this.dEFPAField;
            }
            set
            {
                this.dEFPAField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string KNREF
        {
            get
            {
                return this.kNREFField;
            }
            set
            {
                this.kNREFField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string PARZA
        {
            get
            {
                return this.pARZAField;
            }
            set
            {
                this.pARZAField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class CUSTOMER_PARTNERFS_GETResponse
    {
        
        private E1KNVPM[] eT_E1KNVPMField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public E1KNVPM[] ET_E1KNVPM
        {
            get
            {
                return this.eT_E1KNVPMField;
            }
            set
            {
                this.eT_E1KNVPMField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CUSTOMER_PARTNERFS_GETRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        public GetClientPartnerFS.CUSTOMER_PARTNERFS_GET CUSTOMER_PARTNERFS_GET;
        
        public CUSTOMER_PARTNERFS_GETRequest()
        {
        }
        
        public CUSTOMER_PARTNERFS_GETRequest(GetClientPartnerFS.CUSTOMER_PARTNERFS_GET CUSTOMER_PARTNERFS_GET)
        {
            this.CUSTOMER_PARTNERFS_GET = CUSTOMER_PARTNERFS_GET;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CUSTOMER_PARTNERFS_GETResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        public GetClientPartnerFS.CUSTOMER_PARTNERFS_GETResponse CUSTOMER_PARTNERFS_GETResponse;
        
        public CUSTOMER_PARTNERFS_GETResponse1()
        {
        }
        
        public CUSTOMER_PARTNERFS_GETResponse1(GetClientPartnerFS.CUSTOMER_PARTNERFS_GETResponse CUSTOMER_PARTNERFS_GETResponse)
        {
            this.CUSTOMER_PARTNERFS_GETResponse = CUSTOMER_PARTNERFS_GETResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public interface ZCUSTOMER_PARTNERFS_GETChannel : GetClientPartnerFS.ZCUSTOMER_PARTNERFS_GET, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public partial class ZCUSTOMER_PARTNERFS_GETClient : System.ServiceModel.ClientBase<GetClientPartnerFS.ZCUSTOMER_PARTNERFS_GET>, GetClientPartnerFS.ZCUSTOMER_PARTNERFS_GET
    {
        
        public ZCUSTOMER_PARTNERFS_GETClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GetClientPartnerFS.CUSTOMER_PARTNERFS_GETResponse1 GetClientPartnerFS.ZCUSTOMER_PARTNERFS_GET.CUSTOMER_PARTNERFS_GET(GetClientPartnerFS.CUSTOMER_PARTNERFS_GETRequest request)
        {
            return base.Channel.CUSTOMER_PARTNERFS_GET(request);
        }
        
        public GetClientPartnerFS.CUSTOMER_PARTNERFS_GETResponse CUSTOMER_PARTNERFS_GET(GetClientPartnerFS.CUSTOMER_PARTNERFS_GET CUSTOMER_PARTNERFS_GET1)
        {
            GetClientPartnerFS.CUSTOMER_PARTNERFS_GETRequest inValue = new GetClientPartnerFS.CUSTOMER_PARTNERFS_GETRequest();
            inValue.CUSTOMER_PARTNERFS_GET = CUSTOMER_PARTNERFS_GET1;
            GetClientPartnerFS.CUSTOMER_PARTNERFS_GETResponse1 retVal = ((GetClientPartnerFS.ZCUSTOMER_PARTNERFS_GET)(this)).CUSTOMER_PARTNERFS_GET(inValue);
            return retVal.CUSTOMER_PARTNERFS_GETResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<GetClientPartnerFS.CUSTOMER_PARTNERFS_GETResponse1> GetClientPartnerFS.ZCUSTOMER_PARTNERFS_GET.CUSTOMER_PARTNERFS_GETAsync(GetClientPartnerFS.CUSTOMER_PARTNERFS_GETRequest request)
        {
            return base.Channel.CUSTOMER_PARTNERFS_GETAsync(request);
        }
        
        public System.Threading.Tasks.Task<GetClientPartnerFS.CUSTOMER_PARTNERFS_GETResponse1> CUSTOMER_PARTNERFS_GETAsync(GetClientPartnerFS.CUSTOMER_PARTNERFS_GET CUSTOMER_PARTNERFS_GET)
        {
            GetClientPartnerFS.CUSTOMER_PARTNERFS_GETRequest inValue = new GetClientPartnerFS.CUSTOMER_PARTNERFS_GETRequest();
            inValue.CUSTOMER_PARTNERFS_GET = CUSTOMER_PARTNERFS_GET;
            return ((GetClientPartnerFS.ZCUSTOMER_PARTNERFS_GET)(this)).CUSTOMER_PARTNERFS_GETAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
    }
}
