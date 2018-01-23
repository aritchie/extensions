# ACR Entity Framework Core Extensions

## Inception Interception

* You must inherit from AcrDbContext
* You can then use AcrDbContext.ContextCreated.Subscribe(dbContext => {});

## Events

* You must inherit from AcrDbContext
    * instance.BeforeEach
    * instance.BeforeAll
    * instance.AfterEach
    * instance.AfterAll

## Multi-Tenancy

* Inherit from AcrTenancyContext
* Set your dbcontext's TenantId = <int>;
* This will install a save setter as well as a global query filter

## Soft Deletes
* This will install a save setter as well as a global query filter

## Date/Time Stamping
* This will install a save setter

**KNOWN ISSUES**
* Currently, you can't mix global filters (soft deletes, tenancy, date stamps) - it replaces any existing filter
* Inception interception has not been tested with dbcontext pool