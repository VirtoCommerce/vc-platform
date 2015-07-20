
namespace Shipstation.FulfillmentModule.Web.Helpers
{
    public static class OrderXsdSchema
    {
        public const string orderSchema = @"<?xml version='1.0' encoding='utf-8'?>
<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns='http://example.com/XMLSchema/1.0' targetNamespace='http://example.com/XMLSchema/1.0' elementFormDefault='qualified' attributeFormDefault='unqualified'>
  <xs:element name='Orders'>
    <xs:complexType>
      <xs:sequence>
        <xs:element name='Order' maxOccurs='unbounded' minOccurs='0'>
        <xs:complexType>
          <xs:all>
            <xs:element type='String50' name='OrderID' minOccurs='0'/>
            <xs:element type='String50' name='OrderNumber' />
            <xs:element type='DateTime' name='OrderDate'/>
            <xs:element type='String50' name='OrderStatus'/>
            <xs:element type='DateTime' name='LastModified'/>
            <xs:element type='String100' name='ShippingMethod' minOccurs='0'/>
            <xs:element type='String50' name='PaymentMethod' minOccurs='0'/>
            <xs:element type='xs:float' name='OrderTotal'/>
            <xs:element type='xs:float' name='TaxAmount' minOccurs='0'/>
            <xs:element type='xs:float' name='ShippingAmount' minOccurs='0'/>
            <xs:element type='String1000' name='CustomerNotes' minOccurs='0'/>
            <xs:element type='String1000' name='InternalNotes' minOccurs='0'/>
            <xs:element type='xs:boolean' name='Gift' minOccurs='0'/>
            <xs:element type='String1000' name='GiftMessage' minOccurs='0'/>
            <xs:element type='String100' name='CustomField1' minOccurs='0'/>
            <xs:element type='String100' name='CustomField2' minOccurs='0'/>
            <xs:element type='String100' name='CustomField3' minOccurs='0'/>
            <xs:element name='Customer'>
              <xs:complexType>
                <xs:all>
                  <xs:element type='String50' name='CustomerCode'/>
                  <xs:element name='BillTo'>
                    <xs:complexType>
                      <xs:all>
                        <xs:element type='String100' name='Name'/>
                        <xs:element type='String100' name='Company' minOccurs='0'/>
                        <xs:element type='String50' name='Phone' minOccurs='0'/>
                        <xs:element type='Email' name='Email' minOccurs='0'/>
                      </xs:all>
                    </xs:complexType>
                  </xs:element>
                  <xs:element name='ShipTo'>
                    <xs:complexType>
                      <xs:all>
                        <xs:element type='String100' name='Name'/>
                        <xs:element type='String100' name='Company' minOccurs='0'/>
                        <xs:element type='String100' name='Address1'/>
                        <xs:element type='String100' name='Address2' minOccurs='0'/>
                        <xs:element type='String100' name='City'/>
                        <xs:element type='String100' name='State'/>
                        <xs:element type='String50' name='PostalCode' minOccurs='0'/>
                        <xs:element type='StringExactly2' name='Country'/>
                        <xs:element type='String50' name='Phone' minOccurs='0'/>
                      </xs:all>
                    </xs:complexType>
                  </xs:element>
                </xs:all>
              </xs:complexType>
            </xs:element>
            <xs:element name='Items'>
              <xs:complexType>
                <xs:sequence>
                  <xs:element name='Item' maxOccurs='unbounded' minOccurs='0'>
                    <xs:complexType>
                      <xs:all>
                        <xs:element type='String50' name='LineItemID' minOccurs='0'/>
                        <xs:element type='String100' name='SKU'/>
                        <xs:element type='String200' name='Name'/>
                        <xs:element type='xs:boolean' name='Adjustment' minOccurs='0'/>
                        <xs:element type='xs:anyURI' name='ImageUrl' minOccurs='0'/>
                        <xs:element type='xs:float' name='Weight' minOccurs='0'/>
                        <xs:element name='WeightUnits' minOccurs='0'>
                          <xs:simpleType>
                            <xs:restriction base='xs:string'>
                              <xs:pattern value='pound|pounds|lb|lbs|gram|grams|gm|oz|ounces|Pound|Pounds|Lb|Lbs|Gram|Grams|Gm|Oz|Ounces|POUND|POUNDS|LB|LBS|GRAM|GRAMS|GM|OZ|OUNCES'/>
                              </xs:restriction>
                            </xs:simpleType>
                          </xs:element>
                          <xs:element type='xs:byte' name='Quantity'/>
                          <xs:element type='xs:float' name='UnitPrice'/>
                          <xs:element type='String100' name='Location'/>
                          <xs:element name='Options' minOccurs='0'>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:element name='Option' maxOccurs='10' minOccurs='0'>
                                  <xs:complexType>
                                    <xs:all>
                                      <xs:element type='String30' name='Name'/>
                                      <xs:element type='String30' name='Value'/>
                                      <xs:element type='xs:float' name='Weight' minOccurs='0'/>
                                    </xs:all>
                                  </xs:complexType>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                        </xs:all>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:all>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
      <xs:attribute type='xs:short' name='pages'/>
    </xs:complexType>
  </xs:element>
  <xs:simpleType name='DateTime'>
    <xs:restriction base='xs:string'>
      <xs:pattern value='[0-9][0-9]?/[0-9][0-9]?/[0-9][0-9][0-9]?[0-9]? [0-9][0-9]?:[0-9][0-9]?:?[0-9]?[0-9]?.?[aApP]?[mM]?'/>
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name='Email'>
    <xs:restriction base='xs:string'>
      <xs:pattern value='([a-zA-Z0-9_\.\-])+@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+'/>
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name='StringExactly2'>
    <xs:restriction base='xs:string'>
      <xs:minLength value='2'/>
      <xs:maxLength value='2'/>
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name='String30'>
    <xs:restriction base='xs:string'>
      <xs:maxLength value='30'/>
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name='String50'>
    <xs:restriction base='xs:string'>
      <xs:maxLength value='50'/>
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name='String100'>
    <xs:restriction base='xs:string'>
      <xs:maxLength value='100'/>
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name='String200'>
    <xs:restriction base='xs:string'>
      <xs:maxLength value='200'/>
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name='String1000'>
    <xs:restriction base='xs:string'>
      <xs:maxLength value='1000'/>
    </xs:restriction>
  </xs:simpleType>
</xs:schema>";
    }
}