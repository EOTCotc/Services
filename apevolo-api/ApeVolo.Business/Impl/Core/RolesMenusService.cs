using System.Collections.Generic;
using System.Threading.Tasks;
using ApeVolo.Business.Base;
using ApeVolo.Common.Extention;
using ApeVolo.Entity.Do.Core;
using ApeVolo.IBusiness.Interface.Core;
using ApeVolo.IRepository.Core;

namespace ApeVolo.Business.Impl.Core;

/// <summary>
/// 角色菜单服务
/// </summary>
public class RolesMenusService : BaseServices<RoleMenu>, IRolesMenusService
{
    #region 构造函数

    public RolesMenusService(IRolesMenusRepository rolesMenusRepository)
    {
        BaseDal = rolesMenusRepository;
    }

    #endregion

    #region 基础方法

    public async Task<bool> DeleteAsync(List<long> roleIds)
    {
        var roleMenus = await BaseDal.QueryListAsync(x => roleIds.Contains(x.RoleId));
        return await DeleteEntityListAsync(roleMenus);
    }

    public async Task<bool> CreateAsync(List<RoleMenu> roleMenu)
    {
        return await AddEntityListAsync(roleMenu);
    }

    #endregion
}