INSERT INTO [DisplayTemplateMapping] ([DisplayTemplateMappingId],[Name],[Description],[TargetType],[DisplayTemplate],[Priority],[IsActive],[PredicateSerialized],[PredicateVisualTreeSerialized],[LastModified],[Created],[Discriminator]) VALUES (N'41b589c3-d30f-470d-8acd-48a21bb4f501',N'DefaultCategory',N'Template used to display category',0,N'Category',0,1,NULL,NULL,N'20130327 09:54:14.107',N'20130327 09:54:14.107',N'DisplayTemplateMapping');
INSERT INTO [DisplayTemplateMapping] ([DisplayTemplateMappingId],[Name],[Description],[TargetType],[DisplayTemplate],[Priority],[IsActive],[PredicateSerialized],[PredicateVisualTreeSerialized],[LastModified],[Created],[Discriminator]) VALUES (N'9686f763-a178-48c3-a20c-e9e0721e692c',N'ProductTemplate',N'This template is used to display items',1,N'Item',0,1,N'<LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
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
                    <ConstantExpression NodeType="Constant" CanReduce="false">
                      <Type>
                        <Type Name="System.String" />
                      </Type>
                      <Value>Product</Value>
                    </ConstantExpression>
                  </Right>
                  <Left>
                    <MemberExpression NodeType="MemberAccess" CanReduce="false">
                      <Member MemberType="Property" PropertyName="ItemType">
                        <DeclaringType>
                          <Type Name="VirtoCommerce.Foundation.AppConfig.Model.DisplayTemplateEvaluationContext" />
                        </DeclaringType>
                        <IndexParameters />
                      </Member>
                      <Expression>
                        <UnaryExpression NodeType="Convert" IsLifted="false" IsLiftedToNull="false" CanReduce="false">
                          <Type>
                            <Type Name="VirtoCommerce.Foundation.AppConfig.Model.DisplayTemplateEvaluationContext" />
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
</LambdaExpression>',N'AAEAAAD/////AQAAAAAAAAAMAgAAAF9WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQXBwQ29uZmlnLCBWZXJzaW9uPTEuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAwDAAAAWlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuMS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAwEAAAATldpbmRvd3NCYXNlLCBWZXJzaW9uPTMuMC4wLjAsIEN1bHR1cmU9TmV1dHJhbCwgUHVibGljS2V5VG9rZW49MzFiZjM4NTZhZDM2NGUzNQUBAAAATVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5BcHBDb25maWcuTW9kZWwuRGlzcGxheVRlbXBsYXRlRXhwcmVzc2lvbkJsb2NrCQAAAA9fQ29uZGl0aW9uQmxvY2smVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQEAQEAAAFCVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkFwcENvbmZpZy5Nb2RlbC5Db25kaXRpb25BbmRPckJsb2NrAgAAAD1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50AwAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjEuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS4xLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQECAAAACQUAAAAJBgAAAAkHAAAACQgAAAAKCgAACgUFAAAAQlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5BcHBDb25maWcuTW9kZWwuQ29uZGl0aW9uQW5kT3JCbG9jawkAAAAXPEFsbEFueT5rX19CYWNraW5nRmllbGQmVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfbmV3Q2hpbGRMYWJlbB5Db21wb3NpdGVFbGVtZW50K19lcnJvck1lc3NhZ2UkQ29tcG9zaXRlRWxlbWVudCtfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQEAQEAAAEzVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQWxsQW55AwAAAD1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50AwAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjEuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS4xLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQECAAAACQkAAAAJCgAAAAkLAAAACQwAAAAGDQAAAA8rIGFkZCBjb25kaXRpb24KAAAGDgAAACNpZiAuLi4gb2YgdGhlc2UgY29uZGl0aW9ucyBhcmUgdHJ1ZQUGAAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkNvbXBvc2l0ZUVsZW1lbnQHAAAAD19oZWFkZXJFbGVtZW50cwlfY2hpbGRyZW4OX25ld0NoaWxkTGFiZWwNX2Vycm9yTWVzc2FnZRNfaXNDaGlsZHJlblJlcXVpcmVkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAEBAAAB0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuMS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjEuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAABAQMAAAAJDwAAAAkQAAAABhEAAAALKyBleGNsdWRpbmcKAAAKBQcAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS4xLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0CAAAACF9tb25pdG9yEkNvbGxlY3Rpb25gMStpdGVtcwQD4QFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMStTaW1wbGVNb25pdG9yW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS4xLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAvwFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjEuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAAJEgAAAAkTAAAAAQgAAAAHAAAACRQAAAAJFQAAAAUJAAAAM1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkFsbEFueQcAAAAJVmFsdWVUeXBlMkRpY3Rpb25hcnlFbGVtZW50KzxBdmFpbGFibGVWYWx1ZXM+a19fQmFja2luZ0ZpZWxkHFVzZXJJbnB1dEVsZW1lbnQrX2lucHV0VmFsdWUiVXNlcklucHV0RWxlbWVudCtfaW5wdXREaXNwbGF5TmFtZS5Vc2VySW5wdXRFbGVtZW50KzxEZWZhdWx0VmFsdWU+a19fQmFja2luZ0ZpZWxkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQDBgIBAgABH1N5c3RlbS5Vbml0eVNlcmlhbGl6YXRpb25Ib2xkZXIBAwAAAAkWAAAACRcAAAAKCgYYAAAAA2FsbAAKAQoAAAAGAAAACRkAAAAJGgAAAAkRAAAACgAACgELAAAABwAAAAkcAAAACR0AAAABDAAAAAcAAAAJHgAAAAkfAAAAAQ8AAAAHAAAACSAAAAAJIQAAAAEQAAAABwAAAAkiAAAACSMAAAAFEgAAAOEBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDErU2ltcGxlTW9uaXRvcltbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuMS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dAQAAAApfYnVzeUNvdW50AAgEAAAAAAAAAAQTAAAAvwFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0xLjEuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQMAAAAGX2l0ZW1zBV9zaXplCF92ZXJzaW9uBAAAQFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50W10DAAAACAgJJAAAAAAAAAAAAAAAARQAAAASAAAAAAAAAAEVAAAAEwAAAAklAAAAAQAAAAEAAAAEFgAAAB9TeXN0ZW0uVW5pdHlTZXJpYWxpemF0aW9uSG9sZGVyAwAAAAREYXRhCVVuaXR5VHlwZQxBc3NlbWJseU5hbWUBAAEIBiYAAAANU3lzdGVtLlN0cmluZwQAAAAGJwAAAEttc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODkRFwAAAAIAAAAJGAAAAAYpAAAAA2FueQEZAAAABwAAAAkqAAAACSsAAAABGgAAAAcAAAAJLAAAAAktAAAAARwAAAASAAAAAAAAAAEdAAAAEwAAAAkuAAAAAwAAAAMAAAABHgAAABIAAAAAAAAAAR8AAAATAAAACS8AAAABAAAAAQAAAAEgAAAAEgAAAAAAAAABIQAAABMAAAAJMAAAAAEAAAABAAAAASIAAAASAAAAAAAAAAEjAAAAEwAAAAkxAAAAAAAAAAAAAAAHJAAAAAABAAAAAAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAAByUAAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAkFAAAADQMBKgAAABIAAAAAAAAAASsAAAATAAAACTMAAAABAAAAAQAAAAEsAAAAEgAAAAAAAAABLQAAABMAAAAJMQAAAAAAAAAAAAAABy4AAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAk1AAAACQkAAAAJNwAAAAoHLwAAAAABAAAABAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAACTgAAAANAwcwAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJOQAAAA0DBzEAAAAAAQAAAAAAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAczAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJOgAAAA0DBTUAAAA5VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuTGFiZWxFbGVtZW50AwAAABY8TGFiZWw+a19fQmFja2luZ0ZpZWxkGkV4cHJlc3Npb25FbGVtZW50K19pc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQBAAEBAwAAAAY7AAAAAmlmAAoBNwAAADUAAAAGPAAAABxvZiB0aGVzZSBjb25kaXRpb25zIGFyZSB0cnVlAAoFOAAAAEJWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQXBwQ29uZmlnLk1vZGVsLkNvbmRpdGlvbkl0ZW1UeXBlSXMJAAAACV9pdGVtVHlwZSZUeXBlZEV4cHJlc3Npb25FbGVtZW50QmFzZStfZXhsdWRpbmdFbCBDb21wb3NpdGVFbGVtZW50K19oZWFkZXJFbGVtZW50cxpDb21wb3NpdGVFbGVtZW50K19jaGlsZHJlbh9Db21wb3NpdGVFbGVtZW50K19uZXdDaGlsZExhYmVsHkNvbXBvc2l0ZUVsZW1lbnQrX2Vycm9yTWVzc2FnZSRDb21wb3NpdGVFbGVtZW50K19pc0NoaWxkcmVuUmVxdWlyZWQaRXhwcmVzc2lvbkVsZW1lbnQrX2lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAQEBAQBAQAAAT5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQXBwQ29uZmlnLk1vZGVsLkl0ZW1UeXBlRWxlbWVudAIAAAA9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQ29tcG9zaXRlRWxlbWVudAMAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MS4xLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTEuMS4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAAAEBAgAAAAk9AAAACT4AAAAJPwAAAAlAAAAACgoAAAZBAAAAEkl0ZW0gaXMgb2YgdHlwZSBbXQE5AAAANQAAAAZCAAAACWV4Y2x1ZGluZwAKAToAAAA1AAAACUIAAAAACgU9AAAAPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5BcHBDb25maWcuTW9kZWwuSXRlbVR5cGVFbGVtZW50CAAAAAVJdGVtcwlWYWx1ZVR5cGUyRGljdGlvbmFyeUVsZW1lbnQrPEF2YWlsYWJsZVZhbHVlcz5rX19CYWNraW5nRmllbGQcVXNlcklucHV0RWxlbWVudCtfaW5wdXRWYWx1ZSJVc2VySW5wdXRFbGVtZW50K19pbnB1dERpc3BsYXlOYW1lLlVzZXJJbnB1dEVsZW1lbnQrPERlZmF1bHRWYWx1ZT5rX19CYWNraW5nRmllbGQaRXhwcmVzc2lvbkVsZW1lbnQrX2lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAYDBgIBAgABH1N5c3RlbS5Vbml0eVNlcmlhbGl6YXRpb25Ib2xkZXIBAgAAAAlDAAAACRYAAAAJQwAAAAZGAAAAB1Byb2R1Y3QKBkcAAAADU2t1AAoBPgAAAAYAAAAJSAAAAAlJAAAACREAAAAKAAAKAT8AAAAHAAAACUsAAAAJTAAAAAFAAAAABwAAAAlNAAAACU4AAAARQwAAAAUAAAAJRwAAAAlGAAAABlEAAAAHUGFja2FnZQZSAAAABkJ1bmRsZQZTAAAACkR5bmFtaWNLaXQBSAAAAAcAAAAJVAAAAAlVAAAAAUkAAAAHAAAACVYAAAAJVwAAAAFLAAAAEgAAAAAAAAABTAAAABMAAAAJWAAAAAIAAAACAAAAAU0AAAASAAAAAAAAAAFOAAAAEwAAAAkkAAAAAAAAAAAAAAABVAAAABIAAAAAAAAAAVUAAAATAAAACVoAAAABAAAAAQAAAAFWAAAAEgAAAAAAAAABVwAAABMAAAAJMQAAAAAAAAAAAAAAB1gAAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAlcAAAACT0AAAANAgdaAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJXgAAAA0DAVwAAAA1AAAABl8AAAANSXRlbSB0eXBlIGlzIAAKAV4AAAA1AAAACUIAAAAACgsAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=',N'20130904 08:37:16.427',N'20130327 09:48:14.470',N'DisplayTemplateMapping');
INSERT INTO [DisplayTemplateMapping] ([DisplayTemplateMappingId],[Name],[Description],[TargetType],[DisplayTemplate],[Priority],[IsActive],[PredicateSerialized],[PredicateVisualTreeSerialized],[LastModified],[Created],[Discriminator]) VALUES (N'd5b69b64-cfc2-4021-9aed-fcbbc6df186e',N'VariationTemplate',N'Template for displaying variations',1,N'Item',0,1,N'<LambdaExpression NodeType="Lambda" Name="" TailCall="false" CanReduce="false">
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
                    <ConstantExpression NodeType="Constant" CanReduce="false">
                      <Type>
                        <Type Name="System.String" />
                      </Type>
                      <Value>Sku</Value>
                    </ConstantExpression>
                  </Right>
                  <Left>
                    <MemberExpression NodeType="MemberAccess" CanReduce="false">
                      <Member MemberType="Property" PropertyName="ItemType">
                        <DeclaringType>
                          <Type Name="VirtoCommerce.Foundation.AppConfig.Model.DisplayTemplateEvaluationContext" />
                        </DeclaringType>
                        <IndexParameters />
                      </Member>
                      <Expression>
                        <UnaryExpression NodeType="Convert" IsLifted="false" IsLiftedToNull="false" CanReduce="false">
                          <Type>
                            <Type Name="VirtoCommerce.Foundation.AppConfig.Model.DisplayTemplateEvaluationContext" />
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
</LambdaExpression>',N'AAEAAAD/////AQAAAAAAAAAMAgAAAF9WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQXBwQ29uZmlnLCBWZXJzaW9uPTEuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAwDAAAAWlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTAuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAwEAAAATldpbmRvd3NCYXNlLCBWZXJzaW9uPTMuMC4wLjAsIEN1bHR1cmU9TmV1dHJhbCwgUHVibGljS2V5VG9rZW49MzFiZjM4NTZhZDM2NGUzNQUBAAAATVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5BcHBDb25maWcuTW9kZWwuRGlzcGxheVRlbXBsYXRlRXhwcmVzc2lvbkJsb2NrBwAAAA9fQ29uZGl0aW9uQmxvY2smVHlwZWRFeHByZXNzaW9uRWxlbWVudEJhc2UrX2V4bHVkaW5nRWwgQ29tcG9zaXRlRWxlbWVudCtfaGVhZGVyRWxlbWVudHMaQ29tcG9zaXRlRWxlbWVudCtfY2hpbGRyZW4fQ29tcG9zaXRlRWxlbWVudCtfTmV3Q2hpbGRMYWJlbBpFeHByZXNzaW9uRWxlbWVudCtfSXNWYWxpZC5FeHByZXNzaW9uRWxlbWVudCs8RGlzcGxheU5hbWU+a19fQmFja2luZ0ZpZWxkBAQEBAEAAUJWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQXBwQ29uZmlnLk1vZGVsLkNvbmRpdGlvbkFuZE9yQmxvY2sCAAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkNvbXBvc2l0ZUVsZW1lbnQDAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTAuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0wLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAABAgAAAAkFAAAACQYAAAAJBwAAAAkIAAAACgEKBQUAAABCVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkFwcENvbmZpZy5Nb2RlbC5Db25kaXRpb25BbmRPckJsb2NrBwAAABc8QWxsQW55PmtfX0JhY2tpbmdGaWVsZCZUeXBlZEV4cHJlc3Npb25FbGVtZW50QmFzZStfZXhsdWRpbmdFbCBDb21wb3NpdGVFbGVtZW50K19oZWFkZXJFbGVtZW50cxpDb21wb3NpdGVFbGVtZW50K19jaGlsZHJlbh9Db21wb3NpdGVFbGVtZW50K19OZXdDaGlsZExhYmVsGkV4cHJlc3Npb25FbGVtZW50K19Jc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAQEAQABM1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkFsbEFueQMAAAA9VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuQ29tcG9zaXRlRWxlbWVudAMAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTAuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAAAECAAAACQkAAAAJCgAAAAkLAAAACQwAAAAGDQAAAA8rIGFkZCBjb25kaXRpb24ABg4AAAAjaWYgLi4uIG9mIHRoZXNlIGNvbmRpdGlvbnMgYXJlIHRydWUFBgAAAD1WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5Db21wb3NpdGVFbGVtZW50BQAAAA9faGVhZGVyRWxlbWVudHMJX2NoaWxkcmVuDl9OZXdDaGlsZExhYmVsGkV4cHJlc3Npb25FbGVtZW50K19Jc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQEBAEAAdMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0wLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAAQMAAAAJDwAAAAoGEAAAAAsrIGV4Y2x1ZGluZwEKBQcAAADTAVN5c3RlbS5Db2xsZWN0aW9ucy5PYmplY3RNb2RlbC5PYnNlcnZhYmxlQ29sbGVjdGlvbmAxW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0CAAAACF9tb25pdG9yEkNvbGxlY3Rpb25gMStpdGVtcwQD4QFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMStTaW1wbGVNb25pdG9yW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0EAAAAvwFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0wLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAAJEQAAAAkSAAAAAQgAAAAHAAAACRMAAAAJFAAAAAUJAAAAM1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkFsbEFueQcAAAAJVmFsdWVUeXBlMkRpY3Rpb25hcnlFbGVtZW50KzxBdmFpbGFibGVWYWx1ZXM+a19fQmFja2luZ0ZpZWxkHFVzZXJJbnB1dEVsZW1lbnQrX0lucHV0VmFsdWUiVXNlcklucHV0RWxlbWVudCtfSW5wdXREaXNwbGF5TmFtZS5Vc2VySW5wdXRFbGVtZW50KzxEZWZhdWx0VmFsdWU+a19fQmFja2luZ0ZpZWxkGkV4cHJlc3Npb25FbGVtZW50K19Jc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQDBgIBAgABH1N5c3RlbS5Vbml0eVNlcmlhbGl6YXRpb25Ib2xkZXIBAwAAAAkVAAAACRYAAAAKCgYXAAAAA2FsbAAKAQoAAAAGAAAACRgAAAAKCRAAAAABCgELAAAABwAAAAkaAAAACRsAAAABDAAAAAcAAAAJHAAAAAkdAAAAAQ8AAAAHAAAACR4AAAAJHwAAAAURAAAA4QFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMStTaW1wbGVNb25pdG9yW1tWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudCwgVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUsIFZlcnNpb249MC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0BAAAACl9idXN5Q291bnQACAQAAAAAAAAABBIAAAC/AVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTAuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dAwAAAAZfaXRlbXMFX3NpemUIX3ZlcnNpb24EAABAVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnRbXQMAAAAICAkgAAAAAAAAAAAAAAABEwAAABEAAAAAAAAAARQAAAASAAAACSEAAAABAAAAAQAAAAQVAAAAH1N5c3RlbS5Vbml0eVNlcmlhbGl6YXRpb25Ib2xkZXIDAAAABERhdGEJVW5pdHlUeXBlDEFzc2VtYmx5TmFtZQEAAQgGIgAAAA1TeXN0ZW0uU3RyaW5nBAAAAAYjAAAAS21zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OREWAAAAAgAAAAkXAAAABiUAAAADYW55ARgAAAAHAAAACSYAAAAJJwAAAAEaAAAAEQAAAAAAAAABGwAAABIAAAAJKAAAAAMAAAADAAAAARwAAAARAAAAAAAAAAEdAAAAEgAAAAkpAAAAAQAAAAEAAAABHgAAABEAAAAAAAAAAR8AAAASAAAACSoAAAABAAAAAQAAAAcgAAAAAAEAAAAAAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAHIQAAAAABAAAABAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAACQUAAAANAwEmAAAAEQAAAAAAAAABJwAAABIAAAAJLAAAAAEAAAABAAAABygAAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAktAAAACQkAAAAJLwAAAAoHKQAAAAABAAAABAAAAAQ+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQDAAAACTAAAAANAwcqAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJMQAAAA0DBywAAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAkyAAAADQMFLQAAADlWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5MYWJlbEVsZW1lbnQDAAAAFjxMYWJlbD5rX19CYWNraW5nRmllbGQaRXhwcmVzc2lvbkVsZW1lbnQrX0lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAEAAQEDAAAABjMAAAACaWYACgEvAAAALQAAAAY0AAAAHG9mIHRoZXNlIGNvbmRpdGlvbnMgYXJlIHRydWUACgUwAAAAQlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5BcHBDb25maWcuTW9kZWwuQ29uZGl0aW9uSXRlbVR5cGVJcwcAAAAJX2l0ZW1UeXBlJlR5cGVkRXhwcmVzc2lvbkVsZW1lbnRCYXNlK19leGx1ZGluZ0VsIENvbXBvc2l0ZUVsZW1lbnQrX2hlYWRlckVsZW1lbnRzGkNvbXBvc2l0ZUVsZW1lbnQrX2NoaWxkcmVuH0NvbXBvc2l0ZUVsZW1lbnQrX05ld0NoaWxkTGFiZWwaRXhwcmVzc2lvbkVsZW1lbnQrX0lzVmFsaWQuRXhwcmVzc2lvbkVsZW1lbnQrPERpc3BsYXlOYW1lPmtfX0JhY2tpbmdGaWVsZAQEBAQBAAE+VmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkFwcENvbmZpZy5Nb2RlbC5JdGVtVHlwZUVsZW1lbnQCAAAAPVZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkNvbXBvc2l0ZUVsZW1lbnQDAAAA0wFTeXN0ZW0uQ29sbGVjdGlvbnMuT2JqZWN0TW9kZWwuT2JzZXJ2YWJsZUNvbGxlY3Rpb25gMVtbVmlydG9Db21tZXJjZS5NYW5hZ2VtZW50Q2xpZW50LkNvcmUuQ29udHJvbHMuRXhwcmVzc2lvbkVsZW1lbnQsIFZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLCBWZXJzaW9uPTAuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dBAAAANMBU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsLk9ic2VydmFibGVDb2xsZWN0aW9uYDFbW1ZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50LCBWaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZSwgVmVyc2lvbj0wLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQQAAAABAgAAAAk1AAAACTYAAAAJNwAAAAk4AAAACgAGOQAAABJJdGVtIGlzIG9mIHR5cGUgW10BMQAAAC0AAAAGOgAAAAlleGNsdWRpbmcACgEyAAAALQAAAAk6AAAAAAoFNQAAAD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQXBwQ29uZmlnLk1vZGVsLkl0ZW1UeXBlRWxlbWVudAgAAAAFSXRlbXMJVmFsdWVUeXBlMkRpY3Rpb25hcnlFbGVtZW50KzxBdmFpbGFibGVWYWx1ZXM+a19fQmFja2luZ0ZpZWxkHFVzZXJJbnB1dEVsZW1lbnQrX0lucHV0VmFsdWUiVXNlcklucHV0RWxlbWVudCtfSW5wdXREaXNwbGF5TmFtZS5Vc2VySW5wdXRFbGVtZW50KzxEZWZhdWx0VmFsdWU+a19fQmFja2luZ0ZpZWxkGkV4cHJlc3Npb25FbGVtZW50K19Jc1ZhbGlkLkV4cHJlc3Npb25FbGVtZW50KzxEaXNwbGF5TmFtZT5rX19CYWNraW5nRmllbGQGAwYCAQIAAR9TeXN0ZW0uVW5pdHlTZXJpYWxpemF0aW9uSG9sZGVyAQIAAAAJOwAAAAkVAAAACTsAAAAKCgY+AAAAA1NrdQAKATYAAAAGAAAACT8AAAAKCRAAAAABCgE3AAAABwAAAAlBAAAACUIAAAABOAAAAAcAAAAJQwAAAAlEAAAAETsAAAAFAAAACT4AAAAGRgAAAAdQcm9kdWN0BkcAAAAHUGFja2FnZQZIAAAABkJ1bmRsZQZJAAAACkR5bmFtaWNLaXQBPwAAAAcAAAAJSgAAAAlLAAAAAUEAAAARAAAAAAAAAAFCAAAAEgAAAAlMAAAAAgAAAAIAAAABQwAAABEAAAAAAAAAAUQAAAASAAAACSAAAAAAAAAAAAAAAAFKAAAAEQAAAAAAAAABSwAAABIAAAAJTgAAAAEAAAABAAAAB0wAAAAAAQAAAAQAAAAEPlZpcnRvQ29tbWVyY2UuTWFuYWdlbWVudENsaWVudC5Db3JlLkNvbnRyb2xzLkV4cHJlc3Npb25FbGVtZW50AwAAAAlPAAAACTUAAAANAgdOAAAAAAEAAAAEAAAABD5WaXJ0b0NvbW1lcmNlLk1hbmFnZW1lbnRDbGllbnQuQ29yZS5Db250cm9scy5FeHByZXNzaW9uRWxlbWVudAMAAAAJUQAAAA0DAU8AAAAtAAAABlIAAAANSXRlbSB0eXBlIGlzIAAKAVEAAAAtAAAACToAAAAACgsAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=',N'20130422 12:21:22.557',N'20130328 09:06:16.917',N'DisplayTemplateMapping');
