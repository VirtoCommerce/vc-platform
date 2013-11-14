update [Case] 
set 
--AgentId = (select top 1 [AccountId] FROM [Account]),
--AgentName = (select top 1 UserName FROM [Account]),
CaseTemplateId = (select top 1 CaseTemplateId from [CaseTemplate])