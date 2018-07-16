
-- Roles
Insert into "WebAdminPortal"."Role"( "RoleId", "Status",  "Name") Values(1, 1, '*');
Insert into "WebAdminPortal"."Role"( "RoleId", "Status",  "Name") Values(2, 1, 'PortalAdmin');
Insert into "WebAdminPortal"."Role"( "RoleId", "Status",  "Name") Values(3, 1, 'SiteAdmin');

-- Tabs
Insert into "WebAdminPortal"."Tab"( "TabId", "TabParentId", "TabOrder", "TabType", "Title", "ContentUrl",  "LeftPanelUrl",  "RightPanelUrl", "Roles",  "Status")
Values(1, 0, 1, 1, 'Dashboard', 'dashboard', '', '', '*', 1);
Insert into "WebAdminPortal"."Tab"( "TabId", "TabParentId", "TabOrder", "TabType", "Title", "ContentUrl",  "LeftPanelUrl",  "RightPanelUrl", "Roles",  "Status") 
Values(2, 0, 2, 1, 'Portal Admin', '', '', '', 'PortalAdmin', 1);
Insert into "WebAdminPortal"."Tab"( "TabId", "TabParentId", "TabOrder", "TabType", "Title", "ContentUrl",  "LeftPanelUrl",  "RightPanelUrl", "Roles",  "Status")
Values(3, 2, 1, 3, 'Role Management', 'RoleMgt', '', '', 'PortalAdmin', 1);
Insert into "WebAdminPortal"."Tab"( "TabId", "TabParentId", "TabOrder", "TabType", "Title", "ContentUrl",  "LeftPanelUrl",  "RightPanelUrl", "Roles",  "Status")
Values(4, 2, 2, 3, 'Tab Management', 'TabMgt', '', '', 'PortalAdmin', 1);
Insert into "WebAdminPortal"."Tab"( "TabId", "TabParentId", "TabOrder", "TabType", "Title", "ContentUrl",  "LeftPanelUrl",  "RightPanelUrl", "Roles",  "Status") 
Values(5, 0, 3, 1, 'Site Admin', '', '', '', 'PortalAdmin,SiteAdmin', 1);
Insert into "WebAdminPortal"."Tab"( "TabId", "TabParentId", "TabOrder", "TabType", "Title", "ContentUrl",  "LeftPanelUrl",  "RightPanelUrl", "Roles",  "Status") 
Values(6, 5, 1, 3, 'User Management', 'UserMgt', '', '', 'PortalAdmin,SiteAdmin', 1);
Insert into "WebAdminPortal"."Tab"( "TabId", "TabParentId", "TabOrder", "TabType", "Title", "ContentUrl",  "LeftPanelUrl",  "RightPanelUrl", "Roles",  "Status") 
Values(7, 5, 2, 3, 'Password Reset', 'PassReset', '', '', 'PortalAdmin,SiteAdmin', 1);

-- UserRoles
Insert into "WebAdminPortal"."UserRole"( "UserRoleId", "UserId",  "RoleId") Values(1, 1, 1);
Insert into "WebAdminPortal"."UserRole"( "UserRoleId", "UserId",  "RoleId") Values(2, 1, 2);

