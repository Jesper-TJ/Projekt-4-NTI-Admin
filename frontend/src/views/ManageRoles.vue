<script lang="ts">
import { useManageRoleStore, ITeacher } from "../store/ManageRolesStore"

const manageRoleStore = useManageRoleStore();

interface FetchResponse
{
    items: any;
    total: number;
}

interface FetchDataParams
{
    page: number;
    itemsPerPage: number;
    sortBy: any;
    search: { name: string };
}

interface TableItem
{
    Id: number;
    Name: string;
    Email: string;
    Roles: string[];
}

await manageRoleStore.fetchTeachers() //    Fetches data to store

async function fetchData({ page, itemsPerPage, sortBy, search }: FetchDataParams)
{

    return new Promise<FetchResponse>(resolve => {
        const start = (page - 1) * itemsPerPage
        const end = start + itemsPerPage
        const data = manageRoleStore.allTeachers //    Gets all data from store
        const items = data.slice().filter(item => {
            if (search.name && !item.Name.toLowerCase().includes(search.name.toLowerCase()))
                return false

            return true
        })

        if (sortBy.length)
        {
            const sortKey = sortBy[0].key as keyof ITeacher;
            const sortOrder = sortBy[0].order
            items.sort((a, b) => {
                const aValue = a[sortKey]
                const bValue = b[sortKey]
                if (typeof aValue === "number" && typeof bValue === "number") {
                    return sortOrder === 'desc' ? bValue - aValue : aValue - bValue;
                } else if (typeof aValue === "string" && typeof bValue === "string") {
                    return sortOrder === 'desc'
                        ? bValue.localeCompare(aValue)
                        : aValue.localeCompare(bValue);
                }
                return 0; // If types don't match or are neither number nor string
            })
        }

        const paginated = items.slice(start, end)

        resolve({ items: paginated, total: items.length })
    })
}

export default {
    data: () => ({
        itemsPerPage: 10,
        headers: [
            { title: 'Namn', align: 'start', key: 'Name', sortable: true },
            { title: 'Mail', key: 'Email', align: 'start', sortable: true },
            { title: 'Roles', key: 'Roles', align: 'start', sortable: false},
        ] as const,
        search: '',
        searchName: '',
        serverItems: [] as TableItem[],
        loading: true,
        totalItems: 0,

        roles: ['Personal', 'Bibliotekarie', 'Vaktmästare', 'Rektor', 'Admin'], //    To add more roles, add here
    }),
    watch:
    {
        searchName ()
        {
            this.search = String(Date.now())
        },
    },
    methods:
    {
        loadItems ({ page, itemsPerPage, sortBy }: FetchDataParams)
        {
            this.loading = true
            fetchData({ page, itemsPerPage, sortBy, search: { name: this.searchName } }).then(({ items, total }) => {
                this.serverItems = items
                this.totalItems = total
                this.loading = false
            })
        },
        
        updateRoles(item: ITeacher)
        {
            manageRoleStore.updateRoles(item);
        },
    },
}
</script>
<template>
    <v-data-table-server
    v-model:items-per-page="itemsPerPage"
    :headers="headers"
    :items="serverItems"
    :items-length="totalItems"
    :loading="loading"
    :search="search"
    item-value="name"
    @update:options="loadItems"
    >
    <template #item.Roles="{ item }"> <!-- #item.roles determines which key it falls under in the data table --> <!-- v-model determines which roles from the data is selected -->
        <v-select
        :items="roles"
        v-model="item.Roles"
        label="Roles"
        chips
        base-color="primary"
        variant="outlined"
        item-color="primary"
        multiple
        @update:model-value="updateRoles(item)"
        ></v-select>
    </template>
    <template v-slot:tfoot>
      <tr>
        <td>
          <v-text-field v-model="searchName" class="ma-2" density="compact" placeholder="Search name..." hide-details></v-text-field>
        </td>
      </tr>
    </template>
    </v-data-table-server>
</template>
<style scoped>
</style>