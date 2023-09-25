# interview-task-4create

## Database Updates

### Adding migrations

```powershell
Add-Migration <Migration_Name> -Project CompanyManager.Persistence -StartupProject CompanyManager.Api -Context CompaniesDbContext
```

### Database updates

```powershell
Update-Database -Project CompanyManager.Persistence -StartupProject CompanyManager.Api -Context CompaniesDbContext
```
