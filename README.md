# Асинхронный маппер

Расширяет функционал библиотеки AutoMapper добавляя асинхронность

Примеры использования
[Examples](src/AsyncMapper.Examples)

### Установка
```
Install-Package AsyncMapper
```

Пример:
``` csharp
var person = new Person() { name = "Иван", phone = "+7(916)123-45-67" };
Worker worker = await mapper.Map<Worker>(person);
```

### Конфигурация
Маппер можно настроить двумя способами:

 * `AsyncProfile` - конфигурация находится в отдельных классах
``` csharp
public class ExampleProfile : AsyncProfile {
    public ExampleProfile() {
        /*конфигурация*/
    }
}
```

 * `AsyncMapperConfiguration` - в коде программы
 ``` csharp
var asyncConf = new AsyncMapperConfiguration(cfg => { /*конфигурация*/ });
var mapper = asyncConf.CreateAsyncMapper();
```
В `AsyncMapperConfiguration` можно подключить `AsyncProfile` с помощью `cfg.AddAsyncProfile<TProfile>()`


#### Создание мапы
`CreateAsyncMap` - создать мапу

Конфигурация происходит также, как в AutoMapper. Добавлены следующие расширения:
 * `ForMemberAsync` - конфигурация для отдельного поля/свойства
    * `AddResolver` - использовать резолвер
    * `AddMemberResolver` - использовать резолвер из указанного поля в указанное поле 
 * `AddAsyncResolver` - эквивалент `ForMemberAsync -> AddResolver

Пример
``` csharp
CreateAsyncMap<Person, Worker>()
    .ForMemberAsync(x => x.Phone, o => o.AddResolver<PersonToWorkerResolver>())
    .ForMemberAsync(x => x.Name, o => o.AddMemberResolver<LoginToNameResolver, string>(y => y.Login))
    // конфигурация AutoMapper
    .ForMember(x => x.Id, o => o.MapFrom(y => y.Id))
    .ForMember(x => x.Email, o => o.MapFrom<PersonEmailResolver>())
```
Асинхронная конфигурация может быть смешана с синхронной в любом порядке

**Внимание!** Использование асинхронной конфигурации на синхронной мапе (`CreateMap` из AutoMapper) приведет к ошибке. Асинхронная мапа должна быть создана только через `CreateAsyncMap`.  

### Резолверы
 * `IAsyncValueResolver` - аналог `IValueResolver`
 * `IAsyncMemberValueResolver` - аналог `IMemberValueResolver`

Пример
``` csharp
public class ExampleResolver : IAsyncValueResolver<From, To, int> {
    public async Task<int> Resolve(From source, To destination) {
        /* код */
    }
}

public class ExampleMemberResolver : IAsyncMemberValueResolver<From, To, string, int> {
    public async Task<int> Resolve(From source, To destination, string sourceMember) {
        /* код */
    }
}
```

**Внимание!** В асинхронных резолверах функция `Resolve` должна выполняться **асинхронно**. Если на вашей функции `Resolve` висит варнинг `CS1998`, маппинг может выполняться медленнее.


# Dependency Injection
```
Install-Package AsyncMapper.DependencyInjection
```
#### Добавление сервиса маппера
``` csharp
services.AddAsyncMapper(typeof(ProfileMarkerType));
```
`ProfileMarkerType` - пустой класс, унаследованный от `AsyncProfile`. Используется для обозначения сборки, в которой находятся классы `AsyncProfile` с конфигурацией
```csharp 
ProfileMarkerType : AsyncProfile { }
```

#### Получение маппера
Маппер получается через интерфейс `IAsyncMapper`
``` csharp
ServiceProvider.GetService<IAsyncMapper>()
```

#### Резолверы
DI может быть использовано в резолверах
``` csharp
public class ExampleResolver : IAsyncValueResolver<...> {
    public ExampleResolver(IService service) {
        /* код */
    }
}
```

# Переход проекта с синхронного маппера на асинхронный
Для корректной работы достаточно
 * Заменить сервис маппера в DI
 * Заменить наследование профилей с `Profile` на `AsyncProfile`
 * Маппинг (2 способа)
    * Заменить синхронный маппинг на асинхронный (`mapper.Map(...)` -> `await mapper.Map(...)`)
    * Оставить синхронный маппинг (`mapper.Map(...)` -> `mapper.Sync.Map(...)`)

Для увеличения производительности стоит перевести резолверы на асинхронность где это возможно
