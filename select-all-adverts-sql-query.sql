use [MotoBest]

select
	ap.Name as [Provider],
	a.RemoteId as [Remote Id],
	a.Title as [Title],
	r.Name as [Region],
	town.Name as [Town],
	b.Name as [Brand],
	m.Name as [Model],
	bs.Name as [Body Style],
	t.Type as [Transmission],
	col.Name as [Color],
	e.Type as [Engine],
	con.Type as [Condition],
	es.Type as [Euro Standard],
	a.HorsePowers as [Horse Powers],
	a.Price as [Price],
	format(a.ManufacturingDate, 'MMMM yyyy') as [Manufacturing Date],
	case
		when a.HasFourDoors = 0 then '2/3'
		when a.HasFourDoors = 1 then '4/5'
	end as [Doors],
	a.Views as [Views],
	a.Description,
	a.LastModifiedOn as [Last Modified On],
	a.IsNewImport as [New Import]

from Adverts as a

join AdvertProviders as ap on ap.Id = a.AdvertProviderId
join BodyStyles as bs on bs.Id = a.BodyStyleId
join Brands as b on b.Id = a.BrandId
join Colors as col on col.Id = a.ColorId
join Conditions as con on con.Id = a.ConditionId
join Engines as e on e.Id = a.EngineId
join Models as m on m.Id = a.ModelId
join Regions as r on r.Id = a.RegionId
join Transmissions as t on t.Id = a.TransmissionId
join EuroStandards as es on es.Id = a.EuroStandardId
left join Towns as town on town.Id = a.TownId

order by a.Views desc
select count(*) from Adverts