﻿using ASF.Domain.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 账户登录之后返回信息
    /// </summary>
    public class AccountInfoByLoginResponseDto:IDto
    {
        private AccountInfoByLoginResponseDto()
        {

        }
        public AccountInfoByLoginResponseDto(Account account)
        {
            this.Avatar = account.Avatar;
            this.Name = account.Name;
        }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public RoleInfo Role { get; private set; } = new RoleInfo();

        public class RoleInfo
        {
            public List<PermissionInfo> Permissions { get; private set; } = new List<PermissionInfo>();
        }
        public class PermissionInfo
        {
            public PermissionInfo(Permission permission)
            {
                this.PermissionId = permission.Id;
                this.PermissionName = permission.Name;
                this.Sort = permission.Sort;
            }
            /// <summary>
            /// 权限ID
            /// </summary>
            public string PermissionId { get; set; }
            /// <summary>
            /// 权限名称
            /// </summary>
            public string PermissionName { get; set; }
            /// <summary>
            /// 排序
            /// </summary>
            public int Sort { get; set; }
            /// <summary>
            /// 功能集合
            /// </summary>
            [JsonProperty("actionEntitySet")]
            public List<ActionInfo> Actions { get; private set; } = new List<ActionInfo>();
        }

        public class ActionInfo
        {
            public ActionInfo(Permission permission)
            {
                this.ActionId = permission.Code;
                this.Describe = permission.Name;
            }
            [JsonProperty("action")]
            public string ActionId { get; set; }
            /// <summary>
            /// 描述
            /// </summary>
            public string Describe { get; set; }
        }

    }

}
