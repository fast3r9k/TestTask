--CREATE TABLE Valute
--(
--	ValuteId UNIQUEIDENTIFIER,
--	ValuteName NVARCHAR(50),
--	CharCode NVARCHAR(50)
--)


--CREATE TABLE ValuteTable12
--(
--	Nominal INT,
--	Rate FLOAT,
--	CharCode NVARCHAR(50),
--	RateDate DATE
--)

--CREATE FUNCTION dbo.GetValute (@CHARCODE NVARCHAR(50), @DATE DATE)
--RETURNS @Rate TABLE(Rate FLOAT)
--AS 
--BEGIN
--	INSERT @Rate
--	SELECT Rate FROM ValuteTable1 WITH (NOLOCK)
--	WHERE CharCode = @CHARCODE AND RateDate = @DATE
--	RETURN
--END

--SELECT * FROM dbo.GetValute('EUR','12/12/2019')

