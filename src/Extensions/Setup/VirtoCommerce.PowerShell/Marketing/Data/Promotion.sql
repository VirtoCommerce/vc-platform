INSERT INTO [Promotion] ([PromotionId],[Name],[Description],[Status],[StartDate],[EndDate],[Priority],[PredicateSerialized],[PredicateVisualTreeSerialized],[PerCustomerLimit],[TotalLimit],[ExclusionTypeId],[SegmentSetId],[CouponId],[CouponSetId],[LastModified],[Created],[CatalogId],[StoreId],[Discriminator]) VALUES (N'30ffd001-432a-4b0a-a385-749dcdd73ea9',N'$10 Gift Card',N'$10 Gift card for orders over $100',N'Active',N'20130207 15:17:04.037',NULL,1,N'<LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
  <Type>
    <Type Name="System.Func`2">
      <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
      <Type Name="System.Boolean" />
    </Type>
  </Type>
  <Parameters>
    <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
      <Type>
        <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
      </Type>
    </ParameterExpression>
  </Parameters>
  <Body>
    <BinaryExpression CanReduce="false" IsLifted="false" IsLiftedToNull="false" NodeType="AndAlso">
      <Right>
        <InvocationExpression NodeType="Invoke" CanReduce="false">
          <Type>
            <Type Name="System.Boolean" />
          </Type>
          <Expression>
            <LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
              <Type>
                <Type Name="System.Func`2">
                  <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                  <Type Name="System.Boolean" />
                </Type>
              </Type>
              <Parameters>
                <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
                  <Type>
                    <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                  </Type>
                </ParameterExpression>
              </Parameters>
              <Body>
                <BinaryExpression CanReduce="false" IsLifted="false" IsLiftedToNull="false" NodeType="AndAlso">
                  <Right>
                    <InvocationExpression NodeType="Invoke" CanReduce="false">
                      <Type>
                        <Type Name="System.Boolean" />
                      </Type>
                      <Expression>
                        <LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
                          <Type>
                            <Type Name="System.Func`2">
                              <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                              <Type Name="System.Boolean" />
                            </Type>
                          </Type>
                          <Parameters>
                            <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                              <Type>
                                <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                              </Type>
                            </ParameterExpression>
                          </Parameters>
                          <Body>
                            <BinaryExpression CanReduce="false" IsLifted="false" IsLiftedToNull="false" NodeType="Equal">
                              <Right>
                                <MemberExpression NodeType="MemberAccess" CanReduce="false">
                                  <Member MemberType="Property" PropertyName="Currency">
                                    <DeclaringType>
                                      <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                                    </DeclaringType>
                                    <IndexParameters />
                                  </Member>
                                  <Expression>
                                    <UnaryExpression NodeType="Convert" IsLifted="false" IsLiftedToNull="false" CanReduce="false">
                                      <Type>
                                        <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                                      </Type>
                                      <Operand>
                                        <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                                          <Type>
                                            <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                                          </Type>
                                        </ParameterExpression>
                                      </Operand>
                                      <Method />
                                    </UnaryExpression>
                                  </Expression>
                                  <Type>
                                    <Type Name="System.String" />
                                  </Type>
                                </MemberExpression>
                              </Right>
                              <Left>
                                <ConstantExpression NodeType="Constant" CanReduce="false">
                                  <Type>
                                    <Type Name="System.String" />
                                  </Type>
                                  <Value>USD</Value>
                                </ConstantExpression>
                              </Left>
                              <Method MemberType="Method" MethodName="op_Equality">
                                <DeclaringType>
                                  <Type Name="System.String" />
                                </DeclaringType>
                                <Parameters>
                                  <Type>
                                    <Type Name="System.String" />
                                  </Type>
                                  <Type>
                                    <Type Name="System.String" />
                                  </Type>
                                </Parameters>
                                <GenericArgTypes />
                              </Method>
                              <Conversion />
                              <Type>
                                <Type Name="System.Boolean" />
                              </Type>
                            </BinaryExpression>
                          </Body>
                          <ReturnType>
                            <Type Name="System.Boolean" />
                          </ReturnType>
                        </LambdaExpression>
                      </Expression>
                      <Arguments>
                        <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
                          <Type>
                            <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                          </Type>
                        </ParameterExpression>
                      </Arguments>
                    </InvocationExpression>
                  </Right>
                  <Left>
                    <BinaryExpression CanReduce="false" IsLifted="false" IsLiftedToNull="false" NodeType="AndAlso">
                      <Right>
                        <InvocationExpression NodeType="Invoke" CanReduce="false">
                          <Type>
                            <Type Name="System.Boolean" />
                          </Type>
                          <Expression>
                            <LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
                              <Type>
                                <Type Name="System.Func`2">
                                  <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                                  <Type Name="System.Boolean" />
                                </Type>
                              </Type>
                              <Parameters>
                                <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                                  <Type>
                                    <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                                  </Type>
                                </ParameterExpression>
                              </Parameters>
                              <Body>
                                <BinaryExpression CanReduce="false" IsLifted="false" IsLiftedToNull="false" NodeType="GreaterThanOrEqual">
                                  <Right>
                                    <ConstantExpression NodeType="Constant" CanReduce="false">
                                      <Type>
                                        <Type Name="System.Decimal" />
                                      </Type>
                                      <Value>100</Value>
                                    </ConstantExpression>
                                  </Right>
                                  <Left>
                                    <MethodCallExpression NodeType="Call" CanReduce="false">
                                      <Type>
                                        <Type Name="System.Decimal" />
                                      </Type>
                                      <Method MemberType="Method" MethodName="GetTotalWithExcludings">
                                        <DeclaringType>
                                          <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                                        </DeclaringType>
                                        <Parameters>
                                          <Type>
                                            <Type Name="System.String[]" />
                                          </Type>
                                          <Type>
                                            <Type Name="System.String[]" />
                                          </Type>
                                          <Type>
                                            <Type Name="System.String[]" />
                                          </Type>
                                        </Parameters>
                                        <GenericArgTypes />
                                      </Method>
                                      <Object>
                                        <UnaryExpression NodeType="Convert" IsLifted="false" IsLiftedToNull="false" CanReduce="false">
                                          <Type>
                                            <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                                          </Type>
                                          <Operand>
                                            <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                                              <Type>
                                                <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                                              </Type>
                                            </ParameterExpression>
                                          </Operand>
                                          <Method />
                                        </UnaryExpression>
                                      </Object>
                                      <Arguments>
                                        <NewArrayExpression NodeType="NewArrayInit" CanReduce="false">
                                          <Type>
                                            <Type Name="System.String[]" />
                                          </Type>
                                          <Expressions />
                                        </NewArrayExpression>
                                        <NewArrayExpression NodeType="NewArrayInit" CanReduce="false">
                                          <Type>
                                            <Type Name="System.String[]" />
                                          </Type>
                                          <Expressions />
                                        </NewArrayExpression>
                                        <NewArrayExpression NodeType="NewArrayInit" CanReduce="false">
                                          <Type>
                                            <Type Name="System.String[]" />
                                          </Type>
                                          <Expressions />
                                        </NewArrayExpression>
                                      </Arguments>
                                    </MethodCallExpression>
                                  </Left>
                                  <Method MemberType="Method" MethodName="op_GreaterThanOrEqual">
                                    <DeclaringType>
                                      <Type Name="System.Decimal" />
                                    </DeclaringType>
                                    <Parameters>
                                      <Type>
                                        <Type Name="System.Decimal" />
                                      </Type>
                                      <Type>
                                        <Type Name="System.Decimal" />
                                      </Type>
                                    </Parameters>
                                    <GenericArgTypes />
                                  </Method>
                                  <Conversion />
                                  <Type>
                                    <Type Name="System.Boolean" />
                                  </Type>
                                </BinaryExpression>
                              </Body>
                              <ReturnType>
                                <Type Name="System.Boolean" />
                              </ReturnType>
                            </LambdaExpression>
                          </Expression>
                          <Arguments>
                            <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
                              <Type>
                                <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                              </Type>
                            </ParameterExpression>
                          </Arguments>
                        </InvocationExpression>
                      </Right>
                      <Left>
                        <ConstantExpression NodeType="Constant" CanReduce="false">
                          <Type>
                            <Type Name="System.Boolean" />
                          </Type>
                          <Value>True</Value>
                        </ConstantExpression>
                      </Left>
                      <Method />
                      <Conversion />
                      <Type>
                        <Type Name="System.Boolean" />
                      </Type>
                    </BinaryExpression>
                  </Left>
                  <Method />
                  <Conversion />
                  <Type>
                    <Type Name="System.Boolean" />
                  </Type>
                </BinaryExpression>
              </Body>
              <ReturnType>
                <Type Name="System.Boolean" />
              </ReturnType>
            </LambdaExpression>
          </Expression>
          <Arguments>
            <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
              <Type>
                <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
              </Type>
            </ParameterExpression>
          </Arguments>
        </InvocationExpression>
      </Right>
      <Left>
        <BinaryExpression CanReduce="false" IsLifted="false" IsLiftedToNull="false" NodeType="AndAlso">
          <Right>
            <InvocationExpression NodeType="Invoke" CanReduce="false">
              <Type>
                <Type Name="System.Boolean" />
              </Type>
              <Expression>
                <LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
                  <Type>
                    <Type Name="System.Func`2">
                      <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                      <Type Name="System.Boolean" />
                    </Type>
                  </Type>
                  <Parameters>
                    <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                      <Type>
                        <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                      </Type>
                    </ParameterExpression>
                  </Parameters>
                  <Body>
                    <MemberExpression NodeType="MemberAccess" CanReduce="false">
                      <Member MemberType="Property" PropertyName="IsEveryone">
                        <DeclaringType>
                          <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                        </DeclaringType>
                        <IndexParameters />
                      </Member>
                      <Expression>
                        <UnaryExpression NodeType="Convert" IsLifted="false" IsLiftedToNull="false" CanReduce="false">
                          <Type>
                            <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                          </Type>
                          <Operand>
                            <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                              <Type>
                                <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                              </Type>
                            </ParameterExpression>
                          </Operand>
                          <Method />
                        </UnaryExpression>
                      </Expression>
                      <Type>
                        <Type Name="System.Boolean" />
                      </Type>
                    </MemberExpression>
                  </Body>
                  <ReturnType>
                    <Type Name="System.Boolean" />
                  </ReturnType>
                </LambdaExpression>
              </Expression>
              <Arguments>
                <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
                  <Type>
                    <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                  </Type>
                </ParameterExpression>
              </Arguments>
            </InvocationExpression>
          </Right>
          <Left>
            <ConstantExpression NodeType="Constant" CanReduce="false">
              <Type>
                <Type Name="System.Boolean" />
              </Type>
              <Value>True</Value>
            </ConstantExpression>
          </Left>
          <Method />
          <Conversion />
          <Type>
            <Type Name="System.Boolean" />
          </Type>
        </BinaryExpression>
      </Left>
      <Method />
      <Conversion />
      <Type>
        <Type Name="System.Boolean" />
      </Type>
    </BinaryExpression>
  </Body>
  <ReturnType>
    <Type Name="System.Boolean" />
  </ReturnType>
</LambdaExpression>',N'AAEAAAD/////AQAAAAAAAAAMAgAAAF9WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAwDAAAAWlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAwEAAAATldpbmRvd3NCYXNlLCBWZXJzaW9uPTMuMC4wLjAsIEN1bHR1cmU9TmV1dHJhbCwgUHVibGljS2V5VG9rZW49MzFiZjM4NTZhZDM2NGUzNQUBAAAAS1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQ2FydFByb21vdGlvbkV4cHJlc3Npb25CbG9jawsAAAAeX2NvbmRpdGlvbkN1c3RvbWVyU2VnbWVudEJsb2NrE19jb25kaXRpb25DYXJ0QmxvY2sMX2FjdGlvbkJsb2NrJlR5cGVkRXhwcmVzc2lvbkVsZW1lbnRCYXNlK19leGx1ZGluZ0VsIENvbXBvc2l0ZUVsZW1lbnQrX2hlYWRlckVsZW1lbnRzGkNvbXBvc2l0ZUVsZW1lbnQrX2NoaWxkcmVuH0NvbXBvc2l0ZUVsZW1lbnQrX25ld0NoaWxkTGFiZWweQ29tcG9zaXRlRWxlbWVudCtfZXJyb3JNZXNzYWdlJENvbXBvc2l0ZUVsZW1lbnQrX2lzQ2hpbGRyZW5SZXF1aXJlZBpFeHByZXNzaW9uRWxlbWVudCtfaXNWYWxpZC5FeHByZXNzaW9uRWxlbWVudCs8RGlzcGxheU5hbWU+a19fQmFja2luZ0ZpZWxkBAQEBAQEAQEAAAFCVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5Db25kaXRpb25BbmRPckJsb2NrAgAAAEJWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLk1vZGVsLkNvbmRpdGlvbkFuZE9yQmxvY2sCAAAAOlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQWN0aW9uQmxvY2sCAAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkNvbXBvc2l0ZUVsZW1lbnQDAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAABAQIAAAAJBQAAAAkGAAAACQcAAAAJCAAAAAkJAAAACQoAAAAKCgABCgUFAAAAQlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQ29uZGl0aW9uQW5kT3JCbG9jawkAAAAXPEFsbEFueT5rX19CYWNraW5nRmllbGQmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQEAQEAAAEzVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQWxsQW55AwAAAD1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50AwAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQECAAAACQsAAAAJDAAAAAkNAAAACQ4AAAAGDwAAAA8rIGFkZCB1c2VyZ3JvdXAGEAAAADBDYXJ0IHByb21vdGlvbiByZXF1aXJlcyBhdCBsZWFzdCBvbmUgZWxpZ2liaWxpdHkBAQYRAAAAK0ZvciB2aXNpdG9yIHdpdGggLi4uIG9mIHRoZXNlIGVsaWdpYmlsaXRpZXMBBgAAAAUAAAAJEgAAAAkTAAAACRQAAAAJFQAAAAYWAAAADysgYWRkIGNvbmRpdGlvbgoAAQYXAAAAI2lmIC4uLiBvZiB0aGVzZSBjb25kaXRpb25zIGFyZSB0cnVlBQcAAAA6VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5BY3Rpb25CbG9jawgAAAAmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQBAQAAAT1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50AwAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQECAAAACRgAAAAJGQAAAAkaAAAABhsAAAAMKyBhZGQgZWZmZWN0BhwAAAAmUHJvbW90aW9uIHJlcXVpcmVzIGF0IGxlYXN0IG9uZSByZXdhcmQBAQYdAAAACVRoZXkgZ2V0OgUIAAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkNvbXBvc2l0ZUVsZW1lbnQHAAAAD19oZWFkZXJFbGVtZW50cwlfY2hpbGRyZW4OX25ld0NoaWxkTGFiZWwNX2Vycm9yTWVzc2FnZRNfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAEBAAAB0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAABAQMAAAAJHgAAAAkfAAAABiAAAAALKyBleGNsdWRpbmcKAAEKBQkAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0CAAAACF9tb25pdG9yEkNvbGxlY3Rpb25gMStpdGVtcwQD4QFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMStTaW1wbGVNb25pdG9yW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAvwFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAAJIQAAAAkiAAAAAQoAAAAJAAAACSMAAAAJJAAAAAULAAAAM1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkFsbEFueQkAAAAJVmFsdWVUeXBlMkRpY3Rpb25hcnlFbGVtZW50KzxBdmFpbGFibGVWYWx1ZXM+a19fQmFja2luZ0ZpZWxkHFVzZXJJbnB1dEVsZW1lbnQrX2lucHV0VmFsdWUiVXNlcklucHV0RWxlbWVudCtfaW5wdXREaXNwbGF5TmFtZS5Vc2VySW5wdXRFbGVtZW50KzxEZWZhdWx0VmFsdWU+a19fQmFja2luZ0ZpZWxkKlVzZXJJbnB1dEVsZW1lbnQrPE1heFZhbHVlPmtfX0JhY2tpbmdGaWVsZCpVc2VySW5wdXRFbGVtZW50KzxNaW5WYWx1ZT5rX19CYWNraW5nRmllbGQaRXhwcmVzc2lvbkVsZW1lbnQrX2lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAMGAgECAgIAAR9TeXN0ZW0uVW5pdHlTZXJpYWxpemF0aW9uSG9sZGVyAQMAAAAJJQAAAAkmAAAACgoGJwAAAANhbGwKCgAKAQwAAAAIAAAACSgAAAAJKQAAAAkgAAAACgABCgENAAAACQAAAAkrAAAACSwAAAABDgAAAAkAAAAJLQAAAAkuAAAAARIAAAALAAAACSUAAAAJMAAAAAoKCScAAAAKCgAKARMAAAAIAAAACTIAAAAJMwAAAAkgAAAACgABCgEUAAAACQAAAAk1AAAACTYAAAABFQAAAAkAAAAJNwAAAAk4AAAAARgAAAAIAAAACTkAAAAJOgAAAAkgAAAACgABCgEZAAAACQAAAAk8AAAACT0AAAABGgAAAAkAAAAJPgAAAAk/AAAAAR4AAAAJAAAACUAAAAAJQQAAAAEfAAAACQAAAAlCAAAACUMAAAAFIQAAAOEBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDErU2ltcGxlTW9uaXRvcltbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dAQAAAApfYnVzeUNvdW50AAgEAAAAAAAAAAQiAAAAvwFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQMAAAAGX2l0ZW1zBV9zaXplCF92ZXJzaW9uBAAAQFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50W10DAAAACAgJRAAAAAAAAAAAAAAAASMAAAAhAAAAAAAAAAEkAAAAIgAAAAlFAAAAAwAAAAMAAAAEJQAAAB9TeXN0ZW0uVW5pdHlTZXJpYWxpemF0aW9uSG9sZGVyAwAAAAREYXRhCVVuaXR5VHlwZQxBc3NlbWJseU5hbWUBAAEIBkYAAAANU3lzdGVtLlN0cmluZwQAAAAGRwAAAEttc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODkRJgAAAAIAAAAJJwAAAAZJAAAAA2FueQEoAAAACQAAAAlKAAAACUsAAAABKQAAAAkAAAAJTAAAAAlNAAAAASsAAAAhAAAAAAAAAAEsAAAAIgAAAAlOAAAAAwAAAAMAAAABLQAAACEAAAAAAAAAAS4AAAAiAAAACU8AAAABAAAABQAAABEwAAAAAgAAAAknAAAACUkAAAABMgAAAAkAAAAJUgAAAAlTAAAAATMAAAAJAAAACVQAAAAJVQAAAAE1AAAAIQAAAAAAAAABNgAAACIAAAAJVgAAAAMAAAADAAAAATcAAAAhAAAAAAAAAAE4AAAAIgAAAAlXAAAAAgAAAAoAAAABOQAAAAkAAAAJWAAAAAlZAAAAAToAAAAJAAAACVoAAAAJWwAAAAE8AAAAIQAAAAAAAAABPQAAACIAAAAJXAAAAAEAAAABAAAAAT4AAAAhAAAAAAAAAAE/AAAAIgAAAAldAAAAAQAAABEAAAABQAAAACEAAAAAAAAAAUEAAAAiAAAACV4AAAABAAAAAQAAAAFCAAAAIQAAAAAAAAABQwAAACIAAAAJRAAAAAAAAAAAAAAAB0QAAAAAAQAAAAAAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAdFAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJBQAAAAkGAAAACQcAAAAKAUoAAAAhAAAAAAAAAAFLAAAAIgAAAAljAAAAAQAAAAEAAAABTAAAACEAAAAAAAAAAU0AAAAiAAAACUQAAAAAAAAAAAAAAAdOAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJZQAAAAkLAAAACWcAAAAKB08AAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAloAAAADQMBUgAAACEAAAAAAAAAAVMAAAAiAAAACWkAAAABAAAAAQAAAAFUAAAAIQAAAAAAAAABVQAAACIAAAAJRAAAAAAAAAAAAAAAB1YAAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAlrAAAACRIAAAAJbQAAAAoHVwAAAAABAAAABAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAACW4AAAAJbwAAAA0CAVgAAAAhAAAAAAAAAAFZAAAAIgAAAAlwAAAAAQAAAAEAAAABWgAAACEAAAAAAAAAAVsAAAAiAAAACUQAAAAAAAAAAAAAAAdcAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJcgAAAA0DB10AAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAlzAAAADQMHXgAAAAABAAAABAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAACXQAAAANAwdjAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJdQAAAA0DBWUAAAA5VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuTGFiZWxFbGVtZW50AwAAABY8TGFiZWw+a19fQmFja2luZ0ZpZWxkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQBAAEBAwAAAAZ2AAAAEEZvciB2aXNpdG9yIHdpdGgACgFnAAAAZQAAAAZ3AAAAFm9mIHRoZXNlIGVsaWdpYmlsaXRpZXMACgVoAAAAQlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQ29uZGl0aW9uSXNFdmVyeW9uZQgAAAAmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQBAQAAAT1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50AwAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQECAAAACXgAAAAJeQAAAAl6AAAACgoAAAZ7AAAACEV2ZXJ5b25lB2kAAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAl8AAAADQMBawAAAGUAAAAGfQAAAAJpZgAKAW0AAABlAAAABn4AAAAcb2YgdGhlc2UgY29uZGl0aW9ucyBhcmUgdHJ1ZQAKBW4AAABJVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5Db25kaXRpb25DYXJ0U3VidG90YWxMZWFzdAoAAAALX3N1YlRvdGFsRWwdPEV4YWN0bHlMZWFzdD5rX19CYWNraW5nRmllbGQmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQEBAEBAAABPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLlVzZXJJbnB1dEVsZW1lbnQDAAAAOVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4YWN0bHlMZWFzdAMAAAA9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQ29tcG9zaXRlRWxlbWVudAMAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAAAEBAgAAAAl/AAAACYAAAAAJgQAAAAmCAAAACYMAAAAKCgAABoQAAAATQ2FydCBzdWJ0b3RhbCBpcyBbXQVvAAAAQlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQ29uZGl0aW9uQ3VycmVuY3lJcwkAAAALX2N1cnJlbmN5RWwmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQEAQEAAAE+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRGljdGlvbmFyeUVsZW1lbnQDAAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkNvbXBvc2l0ZUVsZW1lbnQDAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAABAQIAAAAJhQAAAAmGAAAACYcAAAAJiAAAAAoKAAAGiQAAAA5DdXJyZW5jeSBpcyBbXQdwAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJigAAAA0DAXIAAABlAAAACR0AAAAACgVzAAAAUFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQWN0aW9uQ2FydEl0ZW1HZXRGcmVlTnVtSXRlbU9mU2t1CgAAAApfbnVtSXRlbUVsDV9pdGVtc0luU2t1RWwmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQEBAEBAAABPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLlVzZXJJbnB1dEVsZW1lbnQDAAAAOVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuSXRlbXNJblNrdQIAAAA9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQ29tcG9zaXRlRWxlbWVudAMAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAAAEBAgAAAAmMAAAACY0AAAAJjgAAAAmPAAAACZAAAAAKCgAABpEAAAAbR2V0IFtdIGZyZWUgaXRlbXMgb2YgU0tVIFtdAXQAAABlAAAABpIAAAAJZXhjbHVkaW5nAAoBdQAAAGUAAAAJkgAAAAAKAXgAAAAIAAAACZMAAAAJlAAAAAkgAAAACgABCgF5AAAACQAAAAmWAAAACZcAAAABegAAAAkAAAAJmAAAAAmZAAAAAXwAAABlAAAACZIAAAAACgV/AAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLlVzZXJJbnB1dEVsZW1lbnQIAAAACVZhbHVlVHlwZQtfaW5wdXRWYWx1ZRFfaW5wdXREaXNwbGF5TmFtZR08RGVmYXVsdFZhbHVlPmtfX0JhY2tpbmdGaWVsZBk8TWF4VmFsdWU+a19fQmFja2luZ0ZpZWxkGTxNaW5WYWx1ZT5rX19CYWNraW5nRmllbGQaRXhwcmVzc2lvbkVsZW1lbnQrX2lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAMCAQICAgABH1N5c3RlbS5Vbml0eVNlcmlhbGl6YXRpb25Ib2xkZXIBAwAAAAmbAAAACAUDMTAwCggFATAIBR03OTIyODE2MjUxNDI2NDMzNzU5MzU0Mzk1MDMzNQgFATABCgWAAAAAOVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4YWN0bHlMZWFzdAsAAAAHRXhhY3RseQVMZWFzdAlWYWx1ZVR5cGUyRGljdGlvbmFyeUVsZW1lbnQrPEF2YWlsYWJsZVZhbHVlcz5rX19CYWNraW5nRmllbGQcVXNlcklucHV0RWxlbWVudCtfaW5wdXRWYWx1ZSJVc2VySW5wdXRFbGVtZW50K19pbnB1dERpc3BsYXlOYW1lLlVzZXJJbnB1dEVsZW1lbnQrPERlZmF1bHRWYWx1ZT5rX19CYWNraW5nRmllbGQqVXNlcklucHV0RWxlbWVudCs8TWF4VmFsdWU+a19fQmFja2luZ0ZpZWxkKlVzZXJJbnB1dEVsZW1lbnQrPE1pblZhbHVlPmtfX0JhY2tpbmdGaWVsZBpFeHByZXNzaW9uRWxlbWVudCtfaXNWYWxpZC5FeHByZXNzaW9uRWxlbWVudCs8RGlzcGxheU5hbWU+a19fQmFja2luZ0ZpZWxkAQEDBgIBAgICAAEfU3lzdGVtLlVuaXR5U2VyaWFsaXphdGlvbkhvbGRlcgEDAAAABpwAAAAHRXhhY3RseQadAAAACEF0IGxlYXN0CSUAAAAJnwAAAAoKCZ0AAAAKCgAKAYEAAAAIAAAACaEAAAAJogAAAAkgAAAACgABCgGCAAAACQAAAAmkAAAACaUAAAABgwAAAAkAAAAJpgAAAAmnAAAABYUAAAA+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRGljdGlvbmFyeUVsZW1lbnQJAAAAIDxBdmFpbGFibGVWYWx1ZXM+a19fQmFja2luZ0ZpZWxkCVZhbHVlVHlwZRxVc2VySW5wdXRFbGVtZW50K19pbnB1dFZhbHVlIlVzZXJJbnB1dEVsZW1lbnQrX2lucHV0RGlzcGxheU5hbWUuVXNlcklucHV0RWxlbWVudCs8RGVmYXVsdFZhbHVlPmtfX0JhY2tpbmdGaWVsZCpVc2VySW5wdXRFbGVtZW50KzxNYXhWYWx1ZT5rX19CYWNraW5nRmllbGQqVXNlcklucHV0RWxlbWVudCs8TWluVmFsdWU+a19fQmFja2luZ0ZpZWxkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQGAwIBAgICAAEfU3lzdGVtLlVuaXR5U2VyaWFsaXphdGlvbkhvbGRlcgEDAAAACagAAAAJJQAAAAaqAAAAA1VTRAoGqwAAAANKUFkKCgEKAYYAAAAIAAAACawAAAAKCSAAAAAKAAEKAYcAAAAJAAAACa4AAAAJrwAAAAGIAAAACQAAAAmwAAAACbEAAAABigAAAGUAAAAJkgAAAAAKAYwAAAB/AAAACbMAAAAKCggIAQAAAAgI////fwgIAAAAgAAKBY0AAAA5VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5JdGVtc0luU2t1CQAAABBfdmFsdWVTZWxlY3RvckVsJlR5cGVkRXhwcmVzc2lvbkVsZW1lbnRCYXNlK19leGx1ZGluZ0VsIENvbXBvc2l0ZUVsZW1lbnQrX2hlYWRlckVsZW1lbnRzGkNvbXBvc2l0ZUVsZW1lbnQrX2NoaWxkcmVuH0NvbXBvc2l0ZUVsZW1lbnQrX25ld0NoaWxkTGFiZWweQ29tcG9zaXRlRWxlbWVudCtfZXJyb3JNZXNzYWdlJENvbXBvc2l0ZUVsZW1lbnQrX2lzQ2hpbGRyZW5SZXF1aXJlZBpFeHByZXNzaW9uRWxlbWVudCtfaXNWYWxpZC5FeHByZXNzaW9uRWxlbWVudCs8RGlzcGxheU5hbWU+a19fQmFja2luZ0ZpZWxkBAQEBAEBAAABQlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkN1c3RvbVNlbGVjdG9yRWxlbWVudAMAAAA9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQ29tcG9zaXRlRWxlbWVudAMAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAAAEBAgAAAAm0AAAACbUAAAAJtgAAAAm3AAAACgoAAAa4AAAADEl0ZW1zIG9mIFNLVQGOAAAACAAAAAm5AAAACboA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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=',0,0,0,NULL,NULL,NULL,N'20140313 15:02:11.337',N'20130904 06:25:57.307',NULL,N'SampleStore',N'CartPromotion');
INSERT INTO [Promotion] ([PromotionId],[Name],[Description],[Status],[StartDate],[EndDate],[Priority],[PredicateSerialized],[PredicateVisualTreeSerialized],[PerCustomerLimit],[TotalLimit],[ExclusionTypeId],[SegmentSetId],[CouponId],[CouponSetId],[LastModified],[Created],[CatalogId],[StoreId],[Discriminator]) VALUES (N'bd2a753d-4cc7-43a8-a008-4a7c15e5ab6c',N'dsd',NULL,N'Active',N'20140302 14:23:33.067',NULL,1,N'<LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
  <Type>
    <Type Name="System.Func`2">
      <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
      <Type Name="System.Boolean" />
    </Type>
  </Type>
  <Parameters>
    <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
      <Type>
        <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
      </Type>
    </ParameterExpression>
  </Parameters>
  <Body>
    <BinaryExpression CanReduce="false" IsLifted="false" IsLiftedToNull="false" NodeType="AndAlso">
      <Right>
        <InvocationExpression NodeType="Invoke" CanReduce="false">
          <Type>
            <Type Name="System.Boolean" />
          </Type>
          <Expression>
            <LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
              <Type>
                <Type Name="System.Func`2">
                  <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                  <Type Name="System.Boolean" />
                </Type>
              </Type>
              <Parameters>
                <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                  <Type>
                    <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                  </Type>
                </ParameterExpression>
              </Parameters>
              <Body>
                <MethodCallExpression NodeType="Call" CanReduce="false">
                  <Type>
                    <Type Name="System.Boolean" />
                  </Type>
                  <Method MemberType="Method" MethodName="IsItemInProduct">
                    <DeclaringType>
                      <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                    </DeclaringType>
                    <Parameters>
                      <Type>
                        <Type Name="System.String" />
                      </Type>
                    </Parameters>
                    <GenericArgTypes />
                  </Method>
                  <Object>
                    <UnaryExpression NodeType="Convert" IsLifted="false" IsLiftedToNull="false" CanReduce="false">
                      <Type>
                        <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                      </Type>
                      <Operand>
                        <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                          <Type>
                            <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                          </Type>
                        </ParameterExpression>
                      </Operand>
                      <Method />
                    </UnaryExpression>
                  </Object>
                  <Arguments>
                    <ConstantExpression NodeType="Constant" CanReduce="false">
                      <Type>
                        <Type Name="System.String" />
                      </Type>
                      <Value>v-b001f7ahog</Value>
                    </ConstantExpression>
                  </Arguments>
                </MethodCallExpression>
              </Body>
              <ReturnType>
                <Type Name="System.Boolean" />
              </ReturnType>
            </LambdaExpression>
          </Expression>
          <Arguments>
            <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
              <Type>
                <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
              </Type>
            </ParameterExpression>
          </Arguments>
        </InvocationExpression>
      </Right>
      <Left>
        <BinaryExpression CanReduce="false" IsLifted="false" IsLiftedToNull="false" NodeType="AndAlso">
          <Right>
            <InvocationExpression NodeType="Invoke" CanReduce="false">
              <Type>
                <Type Name="System.Boolean" />
              </Type>
              <Expression>
                <LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
                  <Type>
                    <Type Name="System.Func`2">
                      <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                      <Type Name="System.Boolean" />
                    </Type>
                  </Type>
                  <Parameters>
                    <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                      <Type>
                        <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                      </Type>
                    </ParameterExpression>
                  </Parameters>
                  <Body>
                    <MethodCallExpression NodeType="Call" CanReduce="false">
                      <Type>
                        <Type Name="System.Boolean" />
                      </Type>
                      <Method MemberType="Method" MethodName="IsItemInCategory">
                        <DeclaringType>
                          <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                        </DeclaringType>
                        <Parameters>
                          <Type>
                            <Type Name="System.String" />
                          </Type>
                          <Type>
                            <Type Name="System.String[]" />
                          </Type>
                          <Type>
                            <Type Name="System.String[]" />
                          </Type>
                        </Parameters>
                        <GenericArgTypes />
                      </Method>
                      <Object>
                        <UnaryExpression NodeType="Convert" IsLifted="false" IsLiftedToNull="false" CanReduce="false">
                          <Type>
                            <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                          </Type>
                          <Operand>
                            <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                              <Type>
                                <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                              </Type>
                            </ParameterExpression>
                          </Operand>
                          <Method />
                        </UnaryExpression>
                      </Object>
                      <Arguments>
                        <ConstantExpression NodeType="Constant" CanReduce="false">
                          <Type>
                            <Type Name="System.String" />
                          </Type>
                          <Value>e1b56012-d877-4bdd-92d8-3fc186899d0f</Value>
                        </ConstantExpression>
                        <NewArrayExpression NodeType="NewArrayInit" CanReduce="false">
                          <Type>
                            <Type Name="System.String[]" />
                          </Type>
                          <Expressions />
                        </NewArrayExpression>
                        <NewArrayExpression NodeType="NewArrayInit" CanReduce="false">
                          <Type>
                            <Type Name="System.String[]" />
                          </Type>
                          <Expressions />
                        </NewArrayExpression>
                      </Arguments>
                    </MethodCallExpression>
                  </Body>
                  <ReturnType>
                    <Type Name="System.Boolean" />
                  </ReturnType>
                </LambdaExpression>
              </Expression>
              <Arguments>
                <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
                  <Type>
                    <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                  </Type>
                </ParameterExpression>
              </Arguments>
            </InvocationExpression>
          </Right>
          <Left>
            <ConstantExpression NodeType="Constant" CanReduce="false">
              <Type>
                <Type Name="System.Boolean" />
              </Type>
              <Value>True</Value>
            </ConstantExpression>
          </Left>
          <Method />
          <Conversion />
          <Type>
            <Type Name="System.Boolean" />
          </Type>
        </BinaryExpression>
      </Left>
      <Method />
      <Conversion />
      <Type>
        <Type Name="System.Boolean" />
      </Type>
    </BinaryExpression>
  </Body>
  <ReturnType>
    <Type Name="System.Boolean" />
  </ReturnType>
</LambdaExpression>',N'AAEAAAD/////AQAAAAAAAAAMAgAAAF9WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAwDAAAAWlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAwEAAAATldpbmRvd3NCYXNlLCBWZXJzaW9uPTMuMC4wLjAsIEN1bHR1cmU9TmV1dHJhbCwgUHVibGljS2V5VG9rZW49MzFiZjM4NTZhZDM2NGUzNQUBAAAATlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQ2F0YWxvZ1Byb21vdGlvbkV4cHJlc3Npb25CbG9jawoAAAAPX2NvbmRpdGlvbkJsb2NrDF9hY3Rpb25CbG9jayZUeXBlZEV4cHJlc3Npb25FbGVtZW50QmFzZStfZXhsdWRpbmdFbCBDb21wb3NpdGVFbGVtZW50K19oZWFkZXJFbGVtZW50cxpDb21wb3NpdGVFbGVtZW50K19jaGlsZHJlbh9Db21wb3NpdGVFbGVtZW50K19uZXdDaGlsZExhYmVsHkNvbXBvc2l0ZUVsZW1lbnQrX2Vycm9yTWVzc2FnZSRDb21wb3NpdGVFbGVtZW50K19pc0NoaWxkcmVuUmVxdWlyZWQaRXhwcmVzc2lvbkVsZW1lbnQrX2lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAQEBAQEAQEAAAFCVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5Db25kaXRpb25BbmRPckJsb2NrAgAAADpWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLk1vZGVsLkFjdGlvbkJsb2NrAgAAAD1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50AwAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQECAAAACQUAAAAJBgAAAAkHAAAACQgAAAAJCQAAAAoKAAEKBQUAAABCVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5Db25kaXRpb25BbmRPckJsb2NrCQAAABc8QWxsQW55PmtfX0JhY2tpbmdGaWVsZCZUeXBlZEV4cHJlc3Npb25FbGVtZW50QmFzZStfZXhsdWRpbmdFbCBDb21wb3NpdGVFbGVtZW50K19oZWFkZXJFbGVtZW50cxpDb21wb3NpdGVFbGVtZW50K19jaGlsZHJlbh9Db21wb3NpdGVFbGVtZW50K19uZXdDaGlsZExhYmVsHkNvbXBvc2l0ZUVsZW1lbnQrX2Vycm9yTWVzc2FnZSRDb21wb3NpdGVFbGVtZW50K19pc0NoaWxkcmVuUmVxdWlyZWQaRXhwcmVzc2lvbkVsZW1lbnQrX2lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAQEBAQBAQAAATNWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5BbGxBbnkDAAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkNvbXBvc2l0ZUVsZW1lbnQDAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAABAQIAAAAJCgAAAAkLAAAACQwAAAAJDQAAAAYOAAAADysgYWRkIGNvbmRpdGlvbgYPAAAAKVByb21vdGlvbiByZXF1aXJlcyBhdCBsZWFzdCBvbmUgY29uZGl0aW9uAQEGEAAAACNpZiAuLi4gb2YgdGhlc2UgY29uZGl0aW9ucyBhcmUgdHJ1ZQUGAAAAOlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQWN0aW9uQmxvY2sIAAAAJlR5cGVkRXhwcmVzc2lvbkVsZW1lbnRCYXNlK19leGx1ZGluZ0VsIENvbXBvc2l0ZUVsZW1lbnQrX2hlYWRlckVsZW1lbnRzGkNvbXBvc2l0ZUVsZW1lbnQrX2NoaWxkcmVuH0NvbXBvc2l0ZUVsZW1lbnQrX25ld0NoaWxkTGFiZWweQ29tcG9zaXRlRWxlbWVudCtfZXJyb3JNZXNzYWdlJENvbXBvc2l0ZUVsZW1lbnQrX2lzQ2hpbGRyZW5SZXF1aXJlZBpFeHByZXNzaW9uRWxlbWVudCtfaXNWYWxpZC5FeHByZXNzaW9uRWxlbWVudCs8RGlzcGxheU5hbWU+a19fQmFja2luZ0ZpZWxkBAQEAQEAAAE9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQ29tcG9zaXRlRWxlbWVudAMAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAAAEBAgAAAAkRAAAACRIAAAAJEwAAAAYUAAAADCsgYWRkIGVmZmVjdAYVAAAAJlByb21vdGlvbiByZXF1aXJlcyBhdCBsZWFzdCBvbmUgcmV3YXJkAQEGFgAAAAlUaGV5IGdldDoFBwAAAD1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50BwAAAA9faGVhZGVyRWxlbWVudHMJX2NoaWxkcmVuDl9uZXdDaGlsZExhYmVsDV9lcnJvck1lc3NhZ2UTX2lzQ2hpbGRyZW5SZXF1aXJlZBpFeHByZXNzaW9uRWxlbWVudCtfaXNWYWxpZC5FeHByZXNzaW9uRWxlbWVudCs8RGlzcGxheU5hbWU+a19fQmFja2luZ0ZpZWxkBAQBAQAAAdMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQEDAAAACRcAAAAJGAAAAAYZAAAACysgZXhjbHVkaW5nCgABCgUIAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dAgAAAAhfbW9uaXRvchJDb2xsZWN0aW9uYDEraXRlbXMEA+EBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDErU2ltcGxlTW9uaXRvcltbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAAL8BU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuTGlzdGAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAACRoAAAAJGwAAAAEJAAAACAAAAAkcAAAACR0AAAAFCgAAADNWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5BbGxBbnkJAAAACVZhbHVlVHlwZTJEaWN0aW9uYXJ5RWxlbWVudCs8QXZhaWxhYmxlVmFsdWVzPmtfX0JhY2tpbmdGaWVsZBxVc2VySW5wdXRFbGVtZW50K19pbnB1dFZhbHVlIlVzZXJJbnB1dEVsZW1lbnQrX2lucHV0RGlzcGxheU5hbWUuVXNlcklucHV0RWxlbWVudCs8RGVmYXVsdFZhbHVlPmtfX0JhY2tpbmdGaWVsZCpVc2VySW5wdXRFbGVtZW50KzxNYXhWYWx1ZT5rX19CYWNraW5nRmllbGQqVXNlcklucHV0RWxlbWVudCs8TWluVmFsdWU+a19fQmFja2luZ0ZpZWxkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQDBgIBAgICAAEfU3lzdGVtLlVuaXR5U2VyaWFsaXphdGlvbkhvbGRlcgEDAAAACR4AAAAJHwAAAAoKBiAAAAADYWxsCgoACgELAAAABwAAAAkhAAAACSIAAAAJGQAAAAoAAQoBDAAAAAgAAAAJJAAAAAklAAAAAQ0AAAAIAAAACSYAAAAJJwAAAAERAAAABwAAAAkoAAAACSkAAAAJGQAAAAoAAQoBEgAAAAgAAAAJKwAAAAksAAAAARMAAAAIAAAACS0AAAAJLgAAAAEXAAAACAAAAAkvAAAACTAAAAABGAAAAAgAAAAJMQAAAAkyAAAABRoAAADhAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxK1NpbXBsZU1vbml0b3JbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQEAAAAKX2J1c3lDb3VudAAIBAAAAAAAAAAEGwAAAL8BU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuTGlzdGAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0DAAAABl9pdGVtcwVfc2l6ZQhfdmVyc2lvbgQAAEBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudFtdAwAAAAgICTMAAAAAAAAAAAAAAAEcAAAAGgAAAAAAAAABHQAAABsAAAAJNAAAAAIAAAACAAAABB4AAAAfU3lzdGVtLlVuaXR5U2VyaWFsaXphdGlvbkhvbGRlcgMAAAAERGF0YQlVbml0eVR5cGUMQXNzZW1ibHlOYW1lAQABCAY1AAAADVN5c3RlbS5TdHJpbmcEAAAABjYAAABLbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5ER8AAAACAAAACSAAAAAGOAAAAANhbnkBIQAAAAgAAAAJOQAAAAk6AAAAASIAAAAIAAAACTsAAAAJPAAAAAEkAAAAGgAAAAAAAAABJQAAABsAAAAJPQAAAAMAAAADAAAAASYAAAAaAAAAAAAAAAEnAAAAGwAAAAk+AAAAAgAAAAIAAAABKAAAAAgAAAAJPwAAAAlAAAAAASkAAAAIAAAACUEAAAAJQgAAAAErAAAAGgAAAAAAAAABLAAAABsAAAAJQwAAAAEAAAABAAAAAS0AAAAaAAAAAAAAAAEuAAAAGwAAAAlEAAAAAQAAAAEAAAABLwAAABoAAAAAAAAAATAAAAAbAAAACUUAAAABAAAAAQAAAAExAAAAGgAAAAAAAAABMgAAABsAAAAJMwAAAAAAAAAAAAAABzMAAAAAAQAAAAAAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAc0AAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJBQAAAAkGAAAADQIBOQAAABoAAAAAAAAAAToAAAAbAAAACUkAAAABAAAAAQAAAAE7AAAAGgAAAAAAAAABPAAAABsAAAAJMwAAAAAAAAAAAAAABz0AAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAlLAAAACQoAAAAJTQAAAAoHPgAAAAABAAAABAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAACU4AAAAJTwAAAA0CAT8AAAAaAAAAAAAAAAFAAAAAGwAAAAlQAAAAAQAAAAEAAAABQQAAABoAAAAAAAAAAUIAAAAbAAAACTMAAAAAAAAAAAAAAAdDAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJUgAAAA0DB0QAAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAlTAAAADQMHRQAAAAABAAAABAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAACVQAAAANAwdJAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJVQAAAA0DBUsAAAA5VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuTGFiZWxFbGVtZW50AwAAABY8TGFiZWw+a19fQmFja2luZ0ZpZWxkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQBAAEBAwAAAAZWAAAAAmlmAAoBTQAAAEsAAAAGVwAAABxvZiB0aGVzZSBjb25kaXRpb25zIGFyZSB0cnVlAAoFTgAAAEJWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLk1vZGVsLkNvbmRpdGlvbkNhdGVnb3J5SXMJAAAAEl9pdGVtc0luQ2F0ZWdvcnlFbCZUeXBlZEV4cHJlc3Npb25FbGVtZW50QmFzZStfZXhsdWRpbmdFbCBDb21wb3NpdGVFbGVtZW50K19oZWFkZXJFbGVtZW50cxpDb21wb3NpdGVFbGVtZW50K19jaGlsZHJlbh9Db21wb3NpdGVFbGVtZW50K19uZXdDaGlsZExhYmVsHkNvbXBvc2l0ZUVsZW1lbnQrX2Vycm9yTWVzc2FnZSRDb21wb3NpdGVFbGVtZW50K19pc0NoaWxkcmVuUmVxdWlyZWQaRXhwcmVzc2lvbkVsZW1lbnQrX2lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAQEBAQBAQAAAT5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLk1vZGVsLkl0ZW1zSW5DYXRlZ29yeQIAAAA9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQ29tcG9zaXRlRWxlbWVudAMAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAAAEBAgAAAAlYAAAACVkAAAAJWgAAAAlbAAAACgoAAAZcAAAADkNhdGVnb3J5IGlzIFtdBU8AAAA/VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5Db25kaXRpb25FbnRyeUlzCQAAAA9faXRlbXNJbkVudHJ5RWwmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQEAQEAAAE7VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5JdGVtc0luRW50cnkCAAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkNvbXBvc2l0ZUVsZW1lbnQDAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAABAQIAAAAJXQAAAAleAAAACV8AAAAJYAAAAAoKAAAGYQAAAAtFbnRyeSBpcyBbXQdQAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJYgAAAA0DAVIAAABLAAAACRYAAAAACgVTAAAASFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQWN0aW9uQ2F0YWxvZ0l0ZW1HZXRPZlJlbAkAAAAJX2Ftb3VudEVsJlR5cGVkRXhwcmVzc2lvbkVsZW1lbnRCYXNlK19leGx1ZGluZ0VsIENvbXBvc2l0ZUVsZW1lbnQrX2hlYWRlckVsZW1lbnRzGkNvbXBvc2l0ZUVsZW1lbnQrX2NoaWxkcmVuH0NvbXBvc2l0ZUVsZW1lbnQrX25ld0NoaWxkTGFiZWweQ29tcG9zaXRlRWxlbWVudCtfZXJyb3JNZXNzYWdlJENvbXBvc2l0ZUVsZW1lbnQrX2lzQ2hpbGRyZW5SZXF1aXJlZBpFeHByZXNzaW9uRWxlbWVudCtfaXNWYWxpZC5FeHByZXNzaW9uRWxlbWVudCs8RGlzcGxheU5hbWU+a19fQmFja2luZ0ZpZWxkBAQEBAEBAAABPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLlVzZXJJbnB1dEVsZW1lbnQDAAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkNvbXBvc2l0ZUVsZW1lbnQDAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAABAQIAAAAJZAAAAAllAAAACWYAAAAJZwAAAAoKAAAGaAAAAAxHZXQgW10gJSBvZmYBVAAAAEsAAAAGaQAAAAlleGNsdWRpbmcACgFVAAAASwAAAAlpAAAAAAoFWAAAAD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLk1vZGVsLkl0ZW1zSW5DYXRlZ29yeQkAAAAQX3ZhbHVlU2VsZWN0b3JFbCZUeXBlZEV4cHJlc3Npb25FbGVtZW50QmFzZStfZXhsdWRpbmdFbCBDb21wb3NpdGVFbGVtZW50K19oZWFkZXJFbGVtZW50cxpDb21wb3NpdGVFbGVtZW50K19jaGlsZHJlbh9Db21wb3NpdGVFbGVtZW50K19uZXdDaGlsZExhYmVsHkNvbXBvc2l0ZUVsZW1lbnQrX2Vycm9yTWVzc2FnZSRDb21wb3NpdGVFbGVtZW50K19pc0NoaWxkcmVuUmVxdWlyZWQaRXhwcmVzc2lvbkVsZW1lbnQrX2lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAQEBAQBAQAAAUJWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5DdXN0b21TZWxlY3RvckVsZW1lbnQDAAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkNvbXBvc2l0ZUVsZW1lbnQDAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAABAQIAAAAJagAAAAlrAAAACWwAAAAJbQAAAAoKAAAGbgAAABFJdGVtcyBvZiBjYXRlZ29yeQFZAAAABwAAAAlvAAAACXAAAAAJGQAAAAoAAQoBWgAAAAgAAAAJcgAAAAlzAAAAAVsAAAAIAAAACXQAAAAJdQAAAAVdAAAAO1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuSXRlbXNJbkVudHJ5CgAAABBfdmFsdWVTZWxlY3RvckVsE1NlbGVjdGVkSXRlbUNoYW5nZWQmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQEBAEBAAABQlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkN1c3RvbVNlbGVjdG9yRWxlbWVudAMAAABOVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5JdGVtc0luRW50cnkrU2VsZWN0ZWRJdGVtQ2hhbmdlAgAAAD1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50AwAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQECAAAACXYAAAAKCXcAAAAJeAAAAAl5AAAACgoAAAZ6AAAADkl0ZW1zIG9mIGVudHJ5AV4AAAAHAAAACXsAAAAKCRkAAAAKAAEKAV8AAAAIAAAACX0AAAAJfgAAAAFgAAAACAAAAAl/AAAACYAAAAABYgAAAEsAAAAJaQAAAAAKBWQAAAA9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuVXNlcklucHV0RWxlbWVudAgAAAAJVmFsdWVUeXBlC19pbnB1dFZhbHVlEV9pbnB1dERpc3BsYXlOYW1lHTxEZWZhdWx0VmFsdWU+a19fQmFja2luZ0ZpZWxkGTxNYXhWYWx1ZT5rX19CYWNraW5nRmllbGQZPE1pblZhbHVlPmtfX0JhY2tpbmdGaWVsZBpFeHByZXNzaW9uRWxlbWVudCtfaXNWYWxpZC5FeHByZXNzaW9uRWxlbWVudCs8RGlzcGxheU5hbWU+a19fQmFja2luZ0ZpZWxkAwIBAgICAAEfU3lzdGVtLlVuaXR5U2VyaWFsaXphdGlvbkhvbGRlcgEDAAAACYIAAAAICAcAAAAKCAgAAAAACAhkAAAACAgAAAAAAQoBZQAAAAcAAAAJgwAAAAmEAAAACRkAAAAKAAEKAWYAAAAIAAAACYYAAAAJhwAAAAFnAAAACAAAAAmIAAAACYkAAAAFagAAAEJWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5DdXN0b21TZWxlY3RvckVsZW1lbnQIAAAACVZhbHVlVHlwZRxVc2VySW5wdXRFbGVtZW50K19pbnB1dFZhbHVlIlVzZXJJbnB1dEVsZW1lbnQr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aWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAnJAAAACWoAAAANAge3AAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJywAAAA0DAbsAAAAaAAAAAAAAAAG8AAAAGwAAAAnMAAAAAQAAAAEAAAAHvQAAAAABAAAABAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAACc0AAAAJdgAAAA0CB78AAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAnPAAAADQMBwAAAAEsAAAAG0AAAAAlFbnRyeSBpcyAACgfCAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJ0QAAAA0DB8MAAAAAAQAAAAAAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAHEAAAASwAAAAbSAAAAA0dldAAKAcYAAABLAAAABtMAAAAFJSBvZmYACgfHAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJ1AAAAA0DAckAAABLAAAABtUAAAARaXRlbXMgb2YgY2F0ZWdvcnkACgHLAAAASwAAAAbWAAAACWV4Y2x1ZGluZwAKB8wAAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAnXAAAADQMBzQAAAEsAAAAG2AAAAA5pdGVtcyBvZiBlbnRyeQAKAc8AAABLAAAABtkAAAAJZXhjbHVkaW5nAAoB0QAAAEsAAAAJ1gAAAAAKAdQAAABLAAAACdYAAAAACgHXAAAASwAAAAnZAAAAAAoLAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==',0,0,0,NULL,NULL,NULL,N'20140306 22:14:48.230',N'20140303 14:24:01.283',N'VendorVirtual',NULL,N'CatalogPromotion');
INSERT INTO [Promotion] ([PromotionId],[Name],[Description],[Status],[StartDate],[EndDate],[Priority],[PredicateSerialized],[PredicateVisualTreeSerialized],[PerCustomerLimit],[TotalLimit],[ExclusionTypeId],[SegmentSetId],[CouponId],[CouponSetId],[LastModified],[Created],[CatalogId],[StoreId],[Discriminator]) VALUES (N'cf0db89d-cc8f-4444-b17c-56cb05cfff10',N'Coupon discount test',N'Thank you for using our $100 gift coupon',N'Active',N'20130328 14:09:53.620',NULL,1,N'<LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
  <Type>
    <Type Name="System.Func`2">
      <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
      <Type Name="System.Boolean" />
    </Type>
  </Type>
  <Parameters>
    <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
      <Type>
        <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
      </Type>
    </ParameterExpression>
  </Parameters>
  <Body>
    <BinaryExpression CanReduce="false" IsLifted="false" IsLiftedToNull="false" NodeType="AndAlso">
      <Right>
        <InvocationExpression NodeType="Invoke" CanReduce="false">
          <Type>
            <Type Name="System.Boolean" />
          </Type>
          <Expression>
            <LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
              <Type>
                <Type Name="System.Func`2">
                  <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                  <Type Name="System.Boolean" />
                </Type>
              </Type>
              <Parameters>
                <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
                  <Type>
                    <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                  </Type>
                </ParameterExpression>
              </Parameters>
              <Body>
                <ConstantExpression NodeType="Constant" CanReduce="false">
                  <Type>
                    <Type Name="System.Boolean" />
                  </Type>
                  <Value>True</Value>
                </ConstantExpression>
              </Body>
              <ReturnType>
                <Type Name="System.Boolean" />
              </ReturnType>
            </LambdaExpression>
          </Expression>
          <Arguments>
            <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
              <Type>
                <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
              </Type>
            </ParameterExpression>
          </Arguments>
        </InvocationExpression>
      </Right>
      <Left>
        <BinaryExpression CanReduce="false" IsLifted="false" IsLiftedToNull="false" NodeType="AndAlso">
          <Right>
            <InvocationExpression NodeType="Invoke" CanReduce="false">
              <Type>
                <Type Name="System.Boolean" />
              </Type>
              <Expression>
                <LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
                  <Type>
                    <Type Name="System.Func`2">
                      <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                      <Type Name="System.Boolean" />
                    </Type>
                  </Type>
                  <Parameters>
                    <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                      <Type>
                        <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                      </Type>
                    </ParameterExpression>
                  </Parameters>
                  <Body>
                    <MemberExpression NodeType="MemberAccess" CanReduce="false">
                      <Member MemberType="Property" PropertyName="IsEveryone">
                        <DeclaringType>
                          <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                        </DeclaringType>
                        <IndexParameters />
                      </Member>
                      <Expression>
                        <UnaryExpression NodeType="Convert" IsLifted="false" IsLiftedToNull="false" CanReduce="false">
                          <Type>
                            <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                          </Type>
                          <Operand>
                            <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                              <Type>
                                <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                              </Type>
                            </ParameterExpression>
                          </Operand>
                          <Method />
                        </UnaryExpression>
                      </Expression>
                      <Type>
                        <Type Name="System.Boolean" />
                      </Type>
                    </MemberExpression>
                  </Body>
                  <ReturnType>
                    <Type Name="System.Boolean" />
                  </ReturnType>
                </LambdaExpression>
              </Expression>
              <Arguments>
                <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
                  <Type>
                    <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                  </Type>
                </ParameterExpression>
              </Arguments>
            </InvocationExpression>
          </Right>
          <Left>
            <ConstantExpression NodeType="Constant" CanReduce="false">
              <Type>
                <Type Name="System.Boolean" />
              </Type>
              <Value>True</Value>
            </ConstantExpression>
          </Left>
          <Method />
          <Conversion />
          <Type>
            <Type Name="System.Boolean" />
          </Type>
        </BinaryExpression>
      </Left>
      <Method />
      <Conversion />
      <Type>
        <Type Name="System.Boolean" />
      </Type>
    </BinaryExpression>
  </Body>
  <ReturnType>
    <Type Name="System.Boolean" />
  </ReturnType>
</LambdaExpression>',N'AAEAAAD/////AQAAAAAAAAAMAgAAAF9WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLCBWZXJzaW9uPTEuMS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAwDAAAAWlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuMS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAwEAAAATldpbmRvd3NCYXNlLCBWZXJzaW9uPTMuMC4wLjAsIEN1bHR1cmU9TmV1dHJhbCwgUHVibGljS2V5VG9rZW49MzFiZjM4NTZhZDM2NGUzNQUBAAAAS1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQ2FydFByb21vdGlvbkV4cHJlc3Npb25CbG9jawsAAAAeX2NvbmRpdGlvbkN1c3RvbWVyU2VnbWVudEJsb2NrE19jb25kaXRpb25DYXJ0QmxvY2sMX2FjdGlvbkJsb2NrJlR5cGVkRXhwcmVzc2lvbkVsZW1lbnRCYXNlK19leGx1ZGluZ0VsIENvbXBvc2l0ZUVsZW1lbnQrX2hlYWRlckVsZW1lbnRzGkNvbXBvc2l0ZUVsZW1lbnQrX2NoaWxkcmVuH0NvbXBvc2l0ZUVsZW1lbnQrX25ld0NoaWxkTGFiZWweQ29tcG9zaXRlRWxlbWVudCtfZXJyb3JNZXNzYWdlJENvbXBvc2l0ZUVsZW1lbnQrX2lzQ2hpbGRyZW5SZXF1aXJlZBpFeHByZXNzaW9uRWxlbWVudCtfaXNWYWxpZC5FeHByZXNzaW9uRWxlbWVudCs8RGlzcGxheU5hbWU+a19fQmFja2luZ0ZpZWxkBAQEBAQEAQEAAAFCVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5Db25kaXRpb25BbmRPckJsb2NrAgAAAEJWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLk1vZGVsLkNvbmRpdGlvbkFuZE9yQmxvY2sCAAAAOlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQWN0aW9uQmxvY2sCAAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkNvbXBvc2l0ZUVsZW1lbnQDAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuMS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjEuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAABAQIAAAAJBQAAAAkGAAAACQcAAAAJCAAAAAkJAAAACQoAAAAKCgABCgUFAAAAQlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQ29uZGl0aW9uQW5kT3JCbG9jawkAAAAXPEFsbEFueT5rX19CYWNraW5nRmllbGQmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQEAQEAAAEzVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQWxsQW55AwAAAD1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50AwAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjEuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS4xLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQECAAAACQsAAAAJDAAAAAkNAAAACQ4AAAAGDwAAAA8rIGFkZCB1c2VyZ3JvdXAGEAAAADBDYXJ0IHByb21vdGlvbiByZXF1aXJlcyBhdCBsZWFzdCBvbmUgZWxpZ2liaWxpdHkBAQYRAAAAK0ZvciB2aXNpdG9yIHdpdGggLi4uIG9mIHRoZXNlIGVsaWdpYmlsaXRpZXMBBgAAAAUAAAAJEgAAAAkTAAAACRQAAAAJFQAAAAYWAAAADysgYWRkIGNvbmRpdGlvbgoAAQYXAAAAI2lmIC4uLiBvZiB0aGVzZSBjb25kaXRpb25zIGFyZSB0cnVlBQcAAAA6VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5BY3Rpb25CbG9jawgAAAAmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQBAQAAAT1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50AwAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjEuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS4xLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQECAAAACRgAAAAJGQAAAAkaAAAABhsAAAAMKyBhZGQgZWZmZWN0BhwAAAAmUHJvbW90aW9uIHJlcXVpcmVzIGF0IGxlYXN0IG9uZSByZXdhcmQBAQYdAAAACVRoZXkgZ2V0OgUIAAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkNvbXBvc2l0ZUVsZW1lbnQHAAAAD19oZWFkZXJFbGVtZW50cwlfY2hpbGRyZW4OX25ld0NoaWxkTGFiZWwNX2Vycm9yTWVzc2FnZRNfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAEBAAAB0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuMS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjEuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAABAQMAAAAJHgAAAAkfAAAABiAAAAALKyBleGNsdWRpbmcKAAEKBQkAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS4xLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0CAAAACF9tb25pdG9yEkNvbGxlY3Rpb25gMStpdGVtcwQD4QFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMStTaW1wbGVNb25pdG9yW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS4xLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAvwFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjEuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAAJIQAAAAkiAAAAAQoAAAAJAAAACSMAAAAJJAAAAAULAAAAM1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkFsbEFueQkAAAAJVmFsdWVUeXBlMkRpY3Rpb25hcnlFbGVtZW50KzxBdmFpbGFibGVWYWx1ZXM+a19fQmFja2luZ0ZpZWxkHFVzZXJJbnB1dEVsZW1lbnQrX2lucHV0VmFsdWUiVXNlcklucHV0RWxlbWVudCtfaW5wdXREaXNwbGF5TmFtZS5Vc2VySW5wdXRFbGVtZW50KzxEZWZhdWx0VmFsdWU+a19fQmFja2luZ0ZpZWxkKlVzZXJJbnB1dEVsZW1lbnQrPE1heFZhbHVlPmtfX0JhY2tpbmdGaWVsZCpVc2VySW5wdXRFbGVtZW50KzxNaW5WYWx1ZT5rX19CYWNraW5nRmllbGQaRXhwcmVzc2lvbkVsZW1lbnQrX2lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAMGAgECAgIAAR9TeXN0ZW0uVW5pdHlTZXJpYWxpemF0aW9uSG9sZGVyAQMAAAAJJQAAAAkmAAAACgoGJwAAAANhbGwKCgAKAQwAAAAIAAAACSgAAAAJKQAAAAkgAAAACgABCgENAAAACQAAAAkrAAAACSwAAAABDgAAAAkAAAAJLQAAAAkuAAAAARIAAAALAAAACSUAAAAJMAAAAAoKCScAAAAKCgAKARMAAAAIAAAACTIAAAAJMwAAAAkgAAAACgABCgEUAAAACQAAAAk1AAAACTYAAAABFQAAAAkAAAAJNwAAAAk4AAAAARgAAAAIAAAACTkAAAAJOgAAAAkgAAAACgABCgEZAAAACQAAAAk8AAAACT0AAAABGgAAAAkAAAAJPgAAAAk/AAAAAR4AAAAJAAAACUAAAAAJQQAAAAEfAAAACQAAAAlCAAAACUMAAAAFIQAAAOEBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDErU2ltcGxlTW9uaXRvcltbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuMS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dAQAAAApfYnVzeUNvdW50AAgEAAAAAAAAAAQiAAAAvwFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjEuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQMAAAAGX2l0ZW1zBV9zaXplCF92ZXJzaW9uBAAAQFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50W10DAAAACAgJRAAAAAAAAAAAAAAAASMAAAAhAAAAAAAAAAEkAAAAIgAAAAlFAAAAAwAAAAMAAAAEJQAAAB9TeXN0ZW0uVW5pdHlTZXJpYWxpemF0aW9uSG9sZGVyAwAAAAREYXRhCVVuaXR5VHlwZQxBc3NlbWJseU5hbWUBAAEIBkYAAAANU3lzdGVtLlN0cmluZwQAAAAGRwAAAEttc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODkRJgAAAAIAAAAJJwAAAAZJAAAAA2FueQEoAAAACQAAAAlKAAAACUsAAAABKQAAAAkAAAAJTAAAAAlNAAAAASsAAAAhAAAAAAAAAAEsAAAAIgAAAAlOAAAAAwAAAAMAAAABLQAAACEAAAAAAAAAAS4AAAAiAAAACU8AAAABAAAAAQAAABEwAAAAAgAAAAknAAAACUkAAAABMgAAAAkAAAAJUgAAAAlTAAAAATMAAAAJAAAACVQAAAAJVQAAAAE1AAAAIQAAAAAAAAABNgAAACIAAAAJVgAAAAMAAAADAAAAATcAAAAhAAAAAAAAAAE4AAAAIgAAAAlXAAAAAAAAAAAAAAABOQAAAAkAAAAJWAAAAAlZAAAAAToAAAAJAAAACVoAAAAJWwAAAAE8AAAAIQAAAAAAAAABPQAAACIAAAAJXAAAAAEAAAABAAAAAT4AAAAhAAAAAAAAAAE/AAAAIgAAAAldAAAAAQAAAAMAAAABQAAAACEAAAAAAAAAAUEAAAAiAAAACV4AAAABAAAAAQAAAAFCAAAAIQAAAAAAAAABQwAAACIAAAAJRAAAAAAAAAAAAAAAB0QAAAAAAQAAAAAAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAdFAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJBQAAAAkGAAAACQcAAAAKAUoAAAAhAAAAAAAAAAFLAAAAIgAAAAljAAAAAQAAAAEAAAABTAAAACEAAAAAAAAAAU0AAAAiAAAACUQAAAAAAAAAAAAAAAdOAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJZQAAAAkLAAAACWcAAAAKB08AAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAloAAAADQMBUgAAACEAAAAAAAAAAVMAAAAiAAAACWkAAAABAAAAAQAAAAFUAAAAIQAAAAAAAAABVQAAACIAAAAJRAAAAAAAAAAAAAAAB1YAAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAlrAAAACRIAAAAJbQAAAAoHVwAAAAABAAAAAAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAAAVgAAAAhAAAAAAAAAAFZAAAAIgAAAAluAAAAAQAAAAEAAAABWgAAACEAAAAAAAAAAVsAAAAiAAAACUQAAAAAAAAAAAAAAAdcAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJcAAAAA0DB10AAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAlxAAAADQMHXgAAAAABAAAABAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAACXIAAAANAwdjAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJcwAAAA0DBWUAAAA5VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuTGFiZWxFbGVtZW50AwAAABY8TGFiZWw+a19fQmFja2luZ0ZpZWxkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQBAAEBAwAAAAZ0AAAAEEZvciB2aXNpdG9yIHdpdGgACgFnAAAAZQAAAAZ1AAAAFm9mIHRoZXNlIGVsaWdpYmlsaXRpZXMACgVoAAAAQlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQ29uZGl0aW9uSXNFdmVyeW9uZQgAAAAmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQBAQAAAT1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50AwAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjEuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS4xLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQECAAAACXYAAAAJdwAAAAl4AAAACgoAAAZ5AAAACEV2ZXJ5b25lB2kAAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAl6AAAADQMBawAAAGUAAAAGewAAAAJpZgAKAW0AAABlAAAABnwAAAAcb2YgdGhlc2UgY29uZGl0aW9ucyBhcmUgdHJ1ZQAKB24AAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAl9AAAADQMBcAAAAGUAAAAJHQAAAAAKBXEAAABLVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5BY3Rpb25DYXJ0SXRlbUdldE9mQWJzRm9yTnVtCgAAAApfbnVtSXRlbUVsCV9hbW91bnRFbCZUeXBlZEV4cHJlc3Npb25FbGVtZW50QmFzZStfZXhsdWRpbmdFbCBDb21wb3NpdGVFbGVtZW50K19oZWFkZXJFbGVtZW50cxpDb21wb3NpdGVFbGVtZW50K19jaGlsZHJlbh9Db21wb3NpdGVFbGVtZW50K19uZXdDaGlsZExhYmVsHkNvbXBvc2l0ZUVsZW1lbnQrX2Vycm9yTWVzc2FnZSRDb21wb3NpdGVFbGVtZW50K19pc0NoaWxkcmVuUmVxdWlyZWQaRXhwcmVzc2lvbkVsZW1lbnQrX2lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAQEBAQEAQEAAAE9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuVXNlcklucHV0RWxlbWVudAMAAAA9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuVXNlcklucHV0RWxlbWVudAMAAAA9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQ29tcG9zaXRlRWxlbWVudAMAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS4xLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuMS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAAAEBAgAAAAl/AAAACYAAAAAJgQAAAAmCAAAACYMAAAAKCgAABoQAAAAUR2V0ICRbXSBvZmYgW10gaXRlbXMBcgAAAGUAAAAGhQAAAAlleGNsdWRpbmcACgFzAAAAZQAAAAmFAAAAAAoBdgAAAAgAAAAJhgAAAAmHAAAACSAAAAAKAAEKAXcAAAAJAAAACYkAAAAJigAAAAF4AAAACQAAAAmLAAAACYwAAAABegAAAGUAAAAJhQAAAAAKAX0AAABlAAAACYUAAAAACgV/AAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLlVzZXJJbnB1dEVsZW1lbnQIAAAACVZhbHVlVHlwZQtfaW5wdXRWYWx1ZRFfaW5wdXREaXNwbGF5TmFtZR08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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==',0,0,0,NULL,N'1f0ce31c-00a4-468f-8e1b-b9a888052d69',NULL,N'20130911 06:42:16.413',N'20130904 06:25:57.700',NULL,N'SampleStore',N'CartPromotion');
INSERT INTO [Promotion] ([PromotionId],[Name],[Description],[Status],[StartDate],[EndDate],[Priority],[PredicateSerialized],[PredicateVisualTreeSerialized],[PerCustomerLimit],[TotalLimit],[ExclusionTypeId],[SegmentSetId],[CouponId],[CouponSetId],[LastModified],[Created],[CatalogId],[StoreId],[Discriminator]) VALUES (N'f95c7e83-867d-4285-bae7-bf7bd45059ee',N'asdasd',N'sdfsdf',N'Active',N'20140302 21:50:23.417',NULL,1,N'<LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
  <Type>
    <Type Name="System.Func`2">
      <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
      <Type Name="System.Boolean" />
    </Type>
  </Type>
  <Parameters>
    <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
      <Type>
        <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
      </Type>
    </ParameterExpression>
  </Parameters>
  <Body>
    <BinaryExpression CanReduce="false" IsLifted="false" IsLiftedToNull="false" NodeType="AndAlso">
      <Right>
        <InvocationExpression NodeType="Invoke" CanReduce="false">
          <Type>
            <Type Name="System.Boolean" />
          </Type>
          <Expression>
            <LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
              <Type>
                <Type Name="System.Func`2">
                  <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                  <Type Name="System.Boolean" />
                </Type>
              </Type>
              <Parameters>
                <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
                  <Type>
                    <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                  </Type>
                </ParameterExpression>
              </Parameters>
              <Body>
                <BinaryExpression CanReduce="false" IsLifted="false" IsLiftedToNull="false" NodeType="AndAlso">
                  <Right>
                    <InvocationExpression NodeType="Invoke" CanReduce="false">
                      <Type>
                        <Type Name="System.Boolean" />
                      </Type>
                      <Expression>
                        <LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
                          <Type>
                            <Type Name="System.Func`2">
                              <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                              <Type Name="System.Boolean" />
                            </Type>
                          </Type>
                          <Parameters>
                            <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                              <Type>
                                <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                              </Type>
                            </ParameterExpression>
                          </Parameters>
                          <Body>
                            <BinaryExpression CanReduce="false" IsLifted="false" IsLiftedToNull="false" NodeType="GreaterThanOrEqual">
                              <Right>
                                <ConstantExpression NodeType="Constant" CanReduce="false">
                                  <Type>
                                    <Type Name="System.Decimal" />
                                  </Type>
                                  <Value>1</Value>
                                </ConstantExpression>
                              </Right>
                              <Left>
                                <MethodCallExpression NodeType="Call" CanReduce="false">
                                  <Type>
                                    <Type Name="System.Decimal" />
                                  </Type>
                                  <Method MemberType="Method" MethodName="GetItemsOfProductQuantity">
                                    <DeclaringType>
                                      <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                                    </DeclaringType>
                                    <Parameters>
                                      <Type>
                                        <Type Name="System.String" />
                                      </Type>
                                      <Type>
                                        <Type Name="System.String[]" />
                                      </Type>
                                    </Parameters>
                                    <GenericArgTypes />
                                  </Method>
                                  <Object>
                                    <UnaryExpression NodeType="Convert" IsLifted="false" IsLiftedToNull="false" CanReduce="false">
                                      <Type>
                                        <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                                      </Type>
                                      <Operand>
                                        <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                                          <Type>
                                            <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                                          </Type>
                                        </ParameterExpression>
                                      </Operand>
                                      <Method />
                                    </UnaryExpression>
                                  </Object>
                                  <Arguments>
                                    <ConstantExpression NodeType="Constant" CanReduce="false">
                                      <Type>
                                        <Type Name="System.String" />
                                      </Type>
                                      <Value>v-b004vrj3fq</Value>
                                    </ConstantExpression>
                                    <NewArrayExpression NodeType="NewArrayInit" CanReduce="false">
                                      <Type>
                                        <Type Name="System.String[]" />
                                      </Type>
                                      <Expressions />
                                    </NewArrayExpression>
                                  </Arguments>
                                </MethodCallExpression>
                              </Left>
                              <Method MemberType="Method" MethodName="op_GreaterThanOrEqual">
                                <DeclaringType>
                                  <Type Name="System.Decimal" />
                                </DeclaringType>
                                <Parameters>
                                  <Type>
                                    <Type Name="System.Decimal" />
                                  </Type>
                                  <Type>
                                    <Type Name="System.Decimal" />
                                  </Type>
                                </Parameters>
                                <GenericArgTypes />
                              </Method>
                              <Conversion />
                              <Type>
                                <Type Name="System.Boolean" />
                              </Type>
                            </BinaryExpression>
                          </Body>
                          <ReturnType>
                            <Type Name="System.Boolean" />
                          </ReturnType>
                        </LambdaExpression>
                      </Expression>
                      <Arguments>
                        <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
                          <Type>
                            <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                          </Type>
                        </ParameterExpression>
                      </Arguments>
                    </InvocationExpression>
                  </Right>
                  <Left>
                    <ConstantExpression NodeType="Constant" CanReduce="false">
                      <Type>
                        <Type Name="System.Boolean" />
                      </Type>
                      <Value>True</Value>
                    </ConstantExpression>
                  </Left>
                  <Method />
                  <Conversion />
                  <Type>
                    <Type Name="System.Boolean" />
                  </Type>
                </BinaryExpression>
              </Body>
              <ReturnType>
                <Type Name="System.Boolean" />
              </ReturnType>
            </LambdaExpression>
          </Expression>
          <Arguments>
            <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
              <Type>
                <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
              </Type>
            </ParameterExpression>
          </Arguments>
        </InvocationExpression>
      </Right>
      <Left>
        <BinaryExpression CanReduce="false" IsLifted="false" IsLiftedToNull="false" NodeType="AndAlso">
          <Right>
            <InvocationExpression NodeType="Invoke" CanReduce="false">
              <Type>
                <Type Name="System.Boolean" />
              </Type>
              <Expression>
                <LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
                  <Type>
                    <Type Name="System.Func`2">
                      <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                      <Type Name="System.Boolean" />
                    </Type>
                  </Type>
                  <Parameters>
                    <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                      <Type>
                        <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                      </Type>
                    </ParameterExpression>
                  </Parameters>
                  <Body>
                    <MemberExpression NodeType="MemberAccess" CanReduce="false">
                      <Member MemberType="Property" PropertyName="IsFirstTimeBuyer">
                        <DeclaringType>
                          <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                        </DeclaringType>
                        <IndexParameters />
                      </Member>
                      <Expression>
                        <UnaryExpression NodeType="Convert" IsLifted="false" IsLiftedToNull="false" CanReduce="false">
                          <Type>
                            <Type Name="VirtoCommerce.Foundation.Marketing.Model.PromotionEvaluationContext" />
                          </Type>
                          <Operand>
                            <ParameterExpression NodeType="Parameter" Name="x" IsByRef="false" CanReduce="false">
                              <Type>
                                <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                              </Type>
                            </ParameterExpression>
                          </Operand>
                          <Method />
                        </UnaryExpression>
                      </Expression>
                      <Type>
                        <Type Name="System.Boolean" />
                      </Type>
                    </MemberExpression>
                  </Body>
                  <ReturnType>
                    <Type Name="System.Boolean" />
                  </ReturnType>
                </LambdaExpression>
              </Expression>
              <Arguments>
                <ParameterExpression NodeType="Parameter" Name="f" IsByRef="false" CanReduce="false">
                  <Type>
                    <Type Name="VirtoCommerce.Foundation.Frameworks.IEvaluationContext" />
                  </Type>
                </ParameterExpression>
              </Arguments>
            </InvocationExpression>
          </Right>
          <Left>
            <ConstantExpression NodeType="Constant" CanReduce="false">
              <Type>
                <Type Name="System.Boolean" />
              </Type>
              <Value>True</Value>
            </ConstantExpression>
          </Left>
          <Method />
          <Conversion />
          <Type>
            <Type Name="System.Boolean" />
          </Type>
        </BinaryExpression>
      </Left>
      <Method />
      <Conversion />
      <Type>
        <Type Name="System.Boolean" />
      </Type>
    </BinaryExpression>
  </Body>
  <ReturnType>
    <Type Name="System.Boolean" />
  </ReturnType>
</LambdaExpression>',N'AAEAAAD/////AQAAAAAAAAAMAgAAAF9WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAwDAAAAWlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAwEAAAATldpbmRvd3NCYXNlLCBWZXJzaW9uPTMuMC4wLjAsIEN1bHR1cmU9TmV1dHJhbCwgUHVibGljS2V5VG9rZW49MzFiZjM4NTZhZDM2NGUzNQUBAAAAS1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQ2FydFByb21vdGlvbkV4cHJlc3Npb25CbG9jawsAAAAeX2NvbmRpdGlvbkN1c3RvbWVyU2VnbWVudEJsb2NrE19jb25kaXRpb25DYXJ0QmxvY2sMX2FjdGlvbkJsb2NrJlR5cGVkRXhwcmVzc2lvbkVsZW1lbnRCYXNlK19leGx1ZGluZ0VsIENvbXBvc2l0ZUVsZW1lbnQrX2hlYWRlckVsZW1lbnRzGkNvbXBvc2l0ZUVsZW1lbnQrX2NoaWxkcmVuH0NvbXBvc2l0ZUVsZW1lbnQrX25ld0NoaWxkTGFiZWweQ29tcG9zaXRlRWxlbWVudCtfZXJyb3JNZXNzYWdlJENvbXBvc2l0ZUVsZW1lbnQrX2lzQ2hpbGRyZW5SZXF1aXJlZBpFeHByZXNzaW9uRWxlbWVudCtfaXNWYWxpZC5FeHByZXNzaW9uRWxlbWVudCs8RGlzcGxheU5hbWU+a19fQmFja2luZ0ZpZWxkBAQEBAQEAQEAAAFCVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5Db25kaXRpb25BbmRPckJsb2NrAgAAAEJWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLk1vZGVsLkNvbmRpdGlvbkFuZE9yQmxvY2sCAAAAOlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQWN0aW9uQmxvY2sCAAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkNvbXBvc2l0ZUVsZW1lbnQDAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAABAQIAAAAJBQAAAAkGAAAACQcAAAAJCAAAAAkJAAAACQoAAAAKCgABCgUFAAAAQlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQ29uZGl0aW9uQW5kT3JCbG9jawkAAAAXPEFsbEFueT5rX19CYWNraW5nRmllbGQmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQEAQEAAAEzVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQWxsQW55AwAAAD1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50AwAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQECAAAACQsAAAAJDAAAAAkNAAAACQ4AAAAGDwAAAA8rIGFkZCB1c2VyZ3JvdXAGEAAAADBDYXJ0IHByb21vdGlvbiByZXF1aXJlcyBhdCBsZWFzdCBvbmUgZWxpZ2liaWxpdHkBAQYRAAAAK0ZvciB2aXNpdG9yIHdpdGggLi4uIG9mIHRoZXNlIGVsaWdpYmlsaXRpZXMBBgAAAAUAAAAJEgAAAAkTAAAACRQAAAAJFQAAAAYWAAAADysgYWRkIGNvbmRpdGlvbgoAAQYXAAAAI2lmIC4uLiBvZiB0aGVzZSBjb25kaXRpb25zIGFyZSB0cnVlBQcAAAA6VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5BY3Rpb25CbG9jawgAAAAmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQBAQAAAT1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50AwAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQECAAAACRgAAAAJGQAAAAkaAAAABhsAAAAMKyBhZGQgZWZmZWN0BhwAAAAmUHJvbW90aW9uIHJlcXVpcmVzIGF0IGxlYXN0IG9uZSByZXdhcmQBAQYdAAAACVRoZXkgZ2V0OgUIAAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkNvbXBvc2l0ZUVsZW1lbnQHAAAAD19oZWFkZXJFbGVtZW50cwlfY2hpbGRyZW4OX25ld0NoaWxkTGFiZWwNX2Vycm9yTWVzc2FnZRNfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAEBAAAB0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAABAQMAAAAJHgAAAAkfAAAABiAAAAALKyBleGNsdWRpbmcKAAEKBQkAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0CAAAACF9tb25pdG9yEkNvbGxlY3Rpb25gMStpdGVtcwQD4QFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMStTaW1wbGVNb25pdG9yW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAvwFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAAJIQAAAAkiAAAAAQoAAAAJAAAACSMAAAAJJAAAAAULAAAAM1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkFsbEFueQkAAAAJVmFsdWVUeXBlMkRpY3Rpb25hcnlFbGVtZW50KzxBdmFpbGFibGVWYWx1ZXM+a19fQmFja2luZ0ZpZWxkHFVzZXJJbnB1dEVsZW1lbnQrX2lucHV0VmFsdWUiVXNlcklucHV0RWxlbWVudCtfaW5wdXREaXNwbGF5TmFtZS5Vc2VySW5wdXRFbGVtZW50KzxEZWZhdWx0VmFsdWU+a19fQmFja2luZ0ZpZWxkKlVzZXJJbnB1dEVsZW1lbnQrPE1heFZhbHVlPmtfX0JhY2tpbmdGaWVsZCpVc2VySW5wdXRFbGVtZW50KzxNaW5WYWx1ZT5rX19CYWNraW5nRmllbGQaRXhwcmVzc2lvbkVsZW1lbnQrX2lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAMGAgECAgIAAR9TeXN0ZW0uVW5pdHlTZXJpYWxpemF0aW9uSG9sZGVyAQMAAAAJJQAAAAkmAAAACgoGJwAAAANhbGwKCgAKAQwAAAAIAAAACSgAAAAJKQAAAAkgAAAACgABCgENAAAACQAAAAkrAAAACSwAAAABDgAAAAkAAAAJLQAAAAkuAAAAARIAAAALAAAACSUAAAAJMAAAAAoKCScAAAAKCgAKARMAAAAIAAAACTIAAAAJMwAAAAkgAAAACgABCgEUAAAACQAAAAk1AAAACTYAAAABFQAAAAkAAAAJNwAAAAk4AAAAARgAAAAIAAAACTkAAAAJOgAAAAkgAAAACgABCgEZAAAACQAAAAk8AAAACT0AAAABGgAAAAkAAAAJPgAAAAk/AAAAAR4AAAAJAAAACUAAAAAJQQAAAAEfAAAACQAAAAlCAAAACUMAAAAFIQAAAOEBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDErU2ltcGxlTW9uaXRvcltbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dAQAAAApfYnVzeUNvdW50AAgEAAAAAAAAAAQiAAAAvwFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQMAAAAGX2l0ZW1zBV9zaXplCF92ZXJzaW9uBAAAQFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50W10DAAAACAgJRAAAAAAAAAAAAAAAASMAAAAhAAAAAAAAAAEkAAAAIgAAAAlFAAAAAwAAAAMAAAAEJQAAAB9TeXN0ZW0uVW5pdHlTZXJpYWxpemF0aW9uSG9sZGVyAwAAAAREYXRhCVVuaXR5VHlwZQxBc3NlbWJseU5hbWUBAAEIBkYAAAANU3lzdGVtLlN0cmluZwQAAAAGRwAAAEttc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODkRJgAAAAIAAAAJJwAAAAZJAAAAA2FueQEoAAAACQAAAAlKAAAACUsAAAABKQAAAAkAAAAJTAAAAAlNAAAAASsAAAAhAAAAAAAAAAEsAAAAIgAAAAlOAAAAAwAAAAMAAAABLQAAACEAAAAAAAAAAS4AAAAiAAAACU8AAAABAAAAAQAAABEwAAAAAgAAAAknAAAACUkAAAABMgAAAAkAAAAJUgAAAAlTAAAAATMAAAAJAAAACVQAAAAJVQAAAAE1AAAAIQAAAAAAAAABNgAAACIAAAAJVgAAAAMAAAADAAAAATcAAAAhAAAAAAAAAAE4AAAAIgAAAAlXAAAAAQAAAAMAAAABOQAAAAkAAAAJWAAAAAlZAAAAAToAAAAJAAAACVoAAAAJWwAAAAE8AAAAIQAAAAAAAAABPQAAACIAAAAJXAAAAAEAAAABAAAAAT4AAAAhAAAAAAAAAAE/AAAAIgAAAAldAAAAAQAAAAEAAAABQAAAACEAAAAAAAAAAUEAAAAiAAAACV4AAAABAAAAAQAAAAFCAAAAIQAAAAAAAAABQwAAACIAAAAJRAAAAAAAAAAAAAAAB0QAAAAAAQAAAAAAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAdFAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJBQAAAAkGAAAACQcAAAAKAUoAAAAhAAAAAAAAAAFLAAAAIgAAAAljAAAAAQAAAAEAAAABTAAAACEAAAAAAAAAAU0AAAAiAAAACUQAAAAAAAAAAAAAAAdOAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJZQAAAAkLAAAACWcAAAAKB08AAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAloAAAADQMBUgAAACEAAAAAAAAAAVMAAAAiAAAACWkAAAABAAAAAQAAAAFUAAAAIQAAAAAAAAABVQAAACIAAAAJRAAAAAAAAAAAAAAAB1YAAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAlrAAAACRIAAAAJbQAAAAoHVwAAAAABAAAABAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAACW4AAAANAwFYAAAAIQAAAAAAAAABWQAAACIAAAAJbwAAAAEAAAABAAAAAVoAAAAhAAAAAAAAAAFbAAAAIgAAAAlEAAAAAAAAAAAAAAAHXAAAAAABAAAABAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAACXEAAAANAwddAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJcgAAAA0DB14AAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAlzAAAADQMHYwAAAAABAAAABAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAACXQAAAANAwVlAAAAOVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkxhYmVsRWxlbWVudAMAAAAWPExhYmVsPmtfX0JhY2tpbmdGaWVsZBpFeHByZXNzaW9uRWxlbWVudCtfaXNWYWxpZC5FeHByZXNzaW9uRWxlbWVudCs8RGlzcGxheU5hbWU+a19fQmFja2luZ0ZpZWxkAQABAQMAAAAGdQAAABBGb3IgdmlzaXRvciB3aXRoAAoBZwAAAGUAAAAGdgAAABZvZiB0aGVzZSBlbGlnaWJpbGl0aWVzAAoFaAAAAEhWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLk1vZGVsLkNvbmRpdGlvbklzRmlyc3RUaW1lQnV5ZXIIAAAAJlR5cGVkRXhwcmVzc2lvbkVsZW1lbnRCYXNlK19leGx1ZGluZ0VsIENvbXBvc2l0ZUVsZW1lbnQrX2hlYWRlckVsZW1lbnRzGkNvbXBvc2l0ZUVsZW1lbnQrX2NoaWxkcmVuH0NvbXBvc2l0ZUVsZW1lbnQrX25ld0NoaWxkTGFiZWweQ29tcG9zaXRlRWxlbWVudCtfZXJyb3JNZXNzYWdlJENvbXBvc2l0ZUVsZW1lbnQrX2lzQ2hpbGRyZW5SZXF1aXJlZBpFeHByZXNzaW9uRWxlbWVudCtfaXNWYWxpZC5FeHByZXNzaW9uRWxlbWVudCs8RGlzcGxheU5hbWU+a19fQmFja2luZ0ZpZWxkBAQEAQEAAAE9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQ29tcG9zaXRlRWxlbWVudAMAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAAAEBAgAAAAl3AAAACXgAAAAJeQAAAAoKAAAGegAAABFGaXJzdCB0aW1lIGJ1eWVycwdpAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJewAAAA0DAWsAAABlAAAABnwAAAACaWYACgFtAAAAZQAAAAZ9AAAAHG9mIHRoZXNlIGNvbmRpdGlvbnMgYXJlIHRydWUACgVuAAAAUlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQ29uZGl0aW9uQXROdW1JdGVtc09mRW50cnlBcmVJbkNhcnQMAAAACl9udW1JdGVtRWwPX2l0ZW1zSW5FbnRyeUVsHTxFeGFjdGx5TGVhc3Q+a19fQmFja2luZ0ZpZWxkITxTZWxlY3RlZEl0ZW1UeXBlPmtfX0JhY2tpbmdGaWVsZCZUeXBlZEV4cHJlc3Npb25FbGVtZW50QmFzZStfZXhsdWRpbmdFbCBDb21wb3NpdGVFbGVtZW50K19oZWFkZXJFbGVtZW50cxpDb21wb3NpdGVFbGVtZW50K19jaGlsZHJlbh9Db21wb3NpdGVFbGVtZW50K19uZXdDaGlsZExhYmVsHkNvbXBvc2l0ZUVsZW1lbnQrX2Vycm9yTWVzc2FnZSRDb21wb3NpdGVFbGVtZW50K19pc0NoaWxkcmVuUmVxdWlyZWQaRXhwcmVzc2lvbkVsZW1lbnQrX2lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAQEBAAEBAQBAQAAAT1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Vc2VySW5wdXRFbGVtZW50AwAAADtWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLk1vZGVsLkl0ZW1zSW5FbnRyeQIAAAA5VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhhY3RseUxlYXN0AwAAAAg9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQ29tcG9zaXRlRWxlbWVudAMAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAAAEBAgAAAAl+AAAACX8AAAAJgAAAAAIAAAAJgQAAAAmCAAAACYMAAAAKCgAABoQAAAApW10gW10gaXRlbXMgb2YgZW50cnkgYXJlIGluIHNob3BwaW5nIGNhcnQHbwAAAAABAAAABAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAACYUAAAANAwFxAAAAZQAAAAkdAAAAAAoFcgAAAElWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLk1vZGVsLkFjdGlvbkNhcnRHZXRPZlJlbFN1YnRvdGFsCQAAAAlfYW1vdW50RWwmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQEAQEAAAE9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuVXNlcklucHV0RWxlbWVudAMAAAA9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQ29tcG9zaXRlRWxlbWVudAMAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAAAEBAgAAAAmHAAAACYgAAAAJiQAAAAmKAAAACgoAAAaLAAAAGkdldCBbXSAlIG9mZiBjYXJ0IHN1YnRvdGFsAXMAAABlAAAABowAAAAJZXhjbHVkaW5nAAoBdAAAAGUAAAAJjAAAAAAKAXcAAAAIAAAACY0AAAAJjgAAAAkgAAAACgABCgF4AAAACQAAAAmQAAAACZEAAAABeQAAAAkAAAAJkgAAAAmTAAAAAXsAAABlAAAACYwAAAAACgV+AAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLlVzZXJJbnB1dEVsZW1lbnQIAAAACVZhbHVlVHlwZQtfaW5wdXRWYWx1ZRFfaW5wdXREaXNwbGF5TmFtZR08RGVmYXVsdFZhbHVlPmtfX0JhY2tpbmdGaWVsZBk8TWF4VmFsdWU+a19fQmFja2luZ0ZpZWxkGTxNaW5WYWx1ZT5rX19CYWNraW5nRmllbGQaRXhwcmVzc2lvbkVsZW1lbnQrX2lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAMCAQICAgABH1N5c3RlbS5Vbml0eVNlcmlhbGl6YXRpb25Ib2xkZXIBAwAAAAmVAAAACgoICAEAAAAICP///38ICAAAAAAACgV/AAAAO1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuSXRlbXNJbkVudHJ5CgAAABBfdmFsdWVTZWxlY3RvckVsE1NlbGVjdGVkSXRlbUNoYW5nZWQmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEAwQEBAEBAAABQlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkN1c3RvbVNlbGVjdG9yRWxlbWVudAMAAAAiU3lzdGVtLkRlbGVnYXRlU2VyaWFsaXphdGlvbkhvbGRlcj1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50AwAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjkuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS45LjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQECAAAACZYAAAAJlwAAAAmYAAAACZkAAAAJmgAAAAoKAAAGmwAAAA5JdGVtcyBvZiBlbnRyeQWAAAAAOVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4YWN0bHlMZWFzdAsAAAAHRXhhY3RseQVMZWFzdAlWYWx1ZVR5cGUyRGljdGlvbmFyeUVsZW1lbnQrPEF2YWlsYWJsZVZhbHVlcz5rX19CYWNraW5nRmllbGQcVXNlcklucHV0RWxlbWVudCtfaW5wdXRWYWx1ZSJVc2VySW5wdXRFbGVtZW50K19pbnB1dERpc3BsYXlOYW1lLlVzZXJJbnB1dEVsZW1lbnQrPERlZmF1bHRWYWx1ZT5rX19CYWNraW5nRmllbGQqVXNlcklucHV0RWxlbWVudCs8TWF4VmFsdWU+a19fQmFja2luZ0ZpZWxkKlVzZXJJbnB1dEVsZW1lbnQrPE1pblZhbHVlPmtfX0JhY2tpbmdGaWVsZBpFeHByZXNzaW9uRWxlbWVudCtfaXNWYWxpZC5FeHByZXNzaW9uRWxlbWVudCs8RGlzcGxheU5hbWU+a19fQmFja2luZ0ZpZWxkAQEDBgIBAgICAAEfU3lzdGVtLlVuaXR5U2VyaWFsaXphdGlvbkhvbGRlcgEDAAAABpwAAAAHRXhhY3RseQadAAAACEF0IGxlYXN0CSUAAAAJnwAAAAoKCZ0AAAAKCgAKAYEAAAAIAAAACaEAAAAJogAAAAkgAAAACgABCgGCAAAACQAAAAmkAAAACaUAAAABgwAAAAkAAAAJpgAAAAmnAAAAAYUAAABlAAAACYwAAAAACgGHAAAAfgAAAAmVAAAACAgKAAAACggIAAAAAAgIZAAAAAgIAAAAAAEKAYgAAAAIAAAACaoAAAAJqwAAAAkgAAAACgABCgGJAAAACQAAAAmtAAAACa4AAAABigAAAAkAAAAJrwAAAAmwAAAAAY0AAAAJAAAACbEAAAAJsgAAAAGOAAAACQAAAAmzAAAACbQAAAABkAAAACEAAAAAAAAAAZEAAAAiAAAACbUAAAABAAAAAQAAAAGSAAAAIQAAAAAAAAABkwAAACIAAAAJRAAAAAAAAAAAAAAAAZUAAAAlAAAABrcAAAAMU3lzdGVtLkludDMyBAAAAAlHAAAABZYAAABCVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQ3VzdG9tU2VsZWN0b3JFbGVtZW50CAAAAAlWYWx1ZVR5cGUcVXNlcklucHV0RWxlbWVudCtfaW5wdXRWYWx1ZSJVc2VySW5wdXRFbGVtZW50K19pbnB1dERpc3BsYXlOYW1lLlVzZXJJbnB1dEVsZW1lbnQrPERlZmF1bHRWYWx1ZT5rX19CYWNraW5nRmllbGQqVXNlcklucHV0RWxlbWVudCs8TWF4VmFsdWU+a19fQmFja2luZ0ZpZWxkKlVzZXJJbnB1dEVsZW1lbnQrPE1pblZhbHVlPmtfX0JhY2tpbmdGaWVsZBpFeHByZXNzaW9uRWxlbWVudCtfaXNWYWxpZC5FeHByZXNzaW9uRWxlbWVudCs8RGlzcGxheU5hbWU+a19fQmFja2luZ0ZpZWxkAwIBAgICAAEfU3lzdGVtLlVuaXR5U2VyaWFsaXphdGlvbkhvbGRlcgEDAAAACSUAAAAGugAAAAx2LWIwMDR2cmozZnEGuwAAADRTYW1zdW5nIFVOMTlENDAwMyAxOS1JbmNoIDcyMHAgNjBIeiBMRUQgSERUViAoQmxhY2spBrwAAAAMc2VsZWN0IGVudHJ5CgoBCgSXAAAAIlN5c3RlbS5EZWxlZ2F0ZVNlcmlhbGl6YXRpb25Ib2xkZXIDAAAACERlbGVnYXRlB3RhcmdldDAHbWV0aG9kMAMEAzBTeXN0ZW0uRGVsZWdhdGVTZXJpYWxpemF0aW9uSG9sZGVyK0RlbGVnYXRlRW50cnlSVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50Lk1hcmtldGluZy5Nb2RlbC5Db25kaXRpb25BdE51bUl0ZW1zT2ZFbnRyeUFyZUluQ2FydAIAAAAvU3lzdGVtLlJlZmxlY3Rpb24uTWVtYmVySW5mb1NlcmlhbGl6YXRpb25Ib2xkZXIJvQAAAAluAAAACb8AAAABmAAAAAgAAAAJwAAAAAnBAAAACSAAAAAKAAEKAZkAAAAJAAAACcMAAAAJxAAAAAGaAAAACQAAAAnFAAAACcYAAAARnwAAAAIAAAAJnAAAAAmdAAAAAaEAAAAJAAAACckAAAAJygAAAAGiAAAACQAAAAnLAAAACcwAAAABpAAAACEAAAAAAAAAAaUAAAAiAAAACc0AAAAEAAAABAAAAAGmAAAAIQAAAAAAAAABpwAAACIAAAAJRAAAAAAAAAAAAAAAAaoAAAAJAAAACc8AAAAJ0AAAAAGrAAAACQAAAAnRAAAACdIAAAABrQAAACEAAAAAAAAAAa4AAAAiAAAACdMAAAADAAAAAwAAAAGvAAAAIQAAAAAAAAABsAAAACIAAAAJRAAAAAAAAAAAAAAAAbEAAAAhAAAAAAAAAAGyAAAAIgAAAAnVAAAAAQAAAAEAAAABswAAACEAAAAAAAAAAbQAAAAiAAAACdYAAAAAAAAAAAAAAAe1AAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJ1wAAAA0DBL0AAAAwU3lzdGVtLkRlbGVnYXRlU2VyaWFsaXphdGlvbkhvbGRlcitEZWxlZ2F0ZUVudHJ5BwAAAAR0eXBlCGFzc2VtYmx5BnRhcmdldBJ0YXJnZXRUeXBlQXNzZW1ibHkOdGFyZ2V0VHlwZU5hbWUKbWV0aG9kTmFtZQ1kZWxlZ2F0ZUVudHJ5AQECAQEBAzBTeXN0ZW0uRGVsZWdhdGVTZXJpYWxpemF0aW9uSG9sZGVyK0RlbGVnYXRlRW50cnkG2AAAAE5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLk1vZGVsLkl0ZW1zSW5FbnRyeStTZWxlY3RlZEl0ZW1DaGFuZ2UG2QAAAF9WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuTWFya2V0aW5nLCBWZXJzaW9uPTEuOS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAbaAAAAB3RhcmdldDAJ2QAAAAbcAAAAUlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5NYXJrZXRpbmcuTW9kZWwuQ29uZGl0aW9uQXROdW1JdGVtc09mRW50cnlBcmVJbkNhcnQG3QAAABhTZWxlY3RlZEVudHJ5SXRlbUNoYW5nZWQKBL8AAAAvU3lzdGVtLlJlZmxlY3Rpb24uTWVtYmVySW5mb1NlcmlhbGl6YXRpb25Ib2xkZXIHAAAABE5hbWUMQXNzZW1ibHlOYW1lCUNsYXNzTmFtZQlTaWduYXR1cmUKU2lnbmF0dXJlMgpNZW1iZXJUeXBlEEdlbmVyaWNBcmd1bWVudHMBAQEBAQADCA1TeXN0ZW0uVHlwZVtdCd0AAAAJ2QAAAAncAAAABuAAAAAkVm9pZCBTZWxlY3RlZEVudHJ5SXRlbUNoYW5nZWQoSW50MzIpBuEAAAAyU3lzdGVtLlZvaWQgU2VsZWN0ZWRFbnRyeUl0ZW1DaGFuZ2VkKFN5c3RlbS5JbnQzMikIAAAACgHAAAAACQAAAAniAAAACeMAAAABwQAAAAkAAAAJ5AAAAAnlAAAAAcMAAAAhAAAAAAAAAAHEAAAAIgAAAAnmAAAAAgAAAAIAAAABxQAA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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=',0,0,0,NULL,NULL,NULL,N'20140303 22:03:57.713',N'20140303 21:52:51.567',NULL,N'SampleStore',N'CartPromotion');
