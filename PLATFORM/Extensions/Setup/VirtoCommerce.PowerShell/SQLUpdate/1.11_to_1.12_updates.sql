-- Updates from code to id for categories
update [SeoUrlKeyword]
set KeywordValue = C.CategoryId
FROM CategoryBase C INNER JOIN [SeoUrlKeyword] O on O.KeywordValue = C.Code
where O.KeywordType = 0

-- Updates from code to id for items
update [SeoUrlKeyword]
set KeywordValue = I.ItemId
FROM Item I INNER JOIN [SeoUrlKeyword] O on O.KeywordValue = I.Code
where O.KeywordType = 1