Select p.*,c.Name AS ColorName, b.Name AS BrandName, pt.Name AS Type, pai.*
from Polishes p
INNER JOIN Colors c
	ON p.ColorID = c.ID
INNER JOIN Brands b
	ON p.BrandID = b.ID
LEFT JOIN Polishes_AdditionalInfo pai
	ON p.ID = pai.PolishID
LEFT JOIN Polishes_PolishTypes ppt
	ON p.ID = ppt.PolishID
LEFT JOIN PolishTypes pt
	ON pt.ID = ppt.PolishTypeID