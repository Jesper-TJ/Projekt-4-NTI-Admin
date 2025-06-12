<script lang="ts">
import { useAccessCardStore, IUsers } from "@/store/AccessCardStore"
import Alert from "@/components/Alert.vue"
import accessCardPanel from "@/components/AccessCardPanel.vue";

const accessCardStore = useAccessCardStore();

interface FetchResponse {
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

await accessCardStore.fetchUsers() //    Fetches data to store

async function fetchData({ page, itemsPerPage, sortBy, search }: FetchDataParams)
{
    return new Promise<FetchResponse>(resolve => {
        const start = (page - 1) * itemsPerPage
        const end = start + itemsPerPage
        const data = accessCardStore.allUsers //    Gets all data from store
        const items = data.slice().filter(item => {
            if (search.name && !item.Name.toLowerCase().includes(search.name.toLowerCase()))
                return false

            return true
        })

        if (sortBy.length)
        {
            const sortKey = sortBy[0].key as keyof IUsers;
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
    components: {
        Alert,
        accessCardPanel,
    },
    data: () => ({
        singleFileUpload: [] as File[],
        bulkFileUpload: [] as File[],
        specificImageUpload: [] as File[],

        accessCards: accessCardStore.userAccessCard,
        
        itemsPerPage: 10,
        
        serverItems: [],
        headers: [
            { title: 'Namn', key: 'Name', align: 'start', sortable: true },
            { title: 'Mail', key: 'Email', align: 'start', sortable: true },
            { title: 'Access Cards', key: 'accessCards', align: 'end', sortable: false },
        ] as const,

        search: '',
        searchName: '',
        loading: true,
        totalItems: 0,
        
        accessCardDisplayOverlay: false,
        getAccessCardDisplayState: false,
        createAccessCardDisplayState: false,
        currentImage: '',
        selectedUser: '',
        selectedUserId: -1,

        alertDisplayState: false,
        alertText: '',
        alertTimeout: 5000
    }),
    methods:
    {
        async createAccessCard()
        {
            accessCardStore.sendImage(this.singleFileUpload)
            await accessCardStore.fetchAccessCardById(this.selectedUserId)
        },

        async bulkUploadImages()
        {
            accessCardStore.sendBulkFile(this.bulkFileUpload)
            await accessCardStore.fetchUsers();
        },

        async createSpecificAccessCard()
        {
            await accessCardStore.sendSpecificImage(this.specificImageUpload, this.selectedUserId);
            this.accessCardDisplayOverlay = !this.accessCardDisplayOverlay;
            this.createAccessCardDisplayState = !this.accessCardDisplayOverlay;
            accessCardStore.fetchAccessCardById(this.selectedUserId);
        },

        loadUsers ({ page, itemsPerPage, sortBy }: FetchDataParams)
        {
            this.loading = true

            fetchData({ page, itemsPerPage, sortBy, search: { name: this.searchName } }).then(({ items, total }) => {
                this.serverItems = items
                this.totalItems = total
                this.loading = false
            })
        },

        async getAccessCard(item: any)
        {
            // Check store if access card is already fetched
            let userAccessCard = this.accessCards.find(e => e.Id == item.Id);

            if(!userAccessCard)
            {
                const wasFound = await accessCardStore.fetchAccessCardById(item.Id);

                if(!wasFound)
                {
                    this.displayAlert("Could not find access card image. The image might not exist yet. Try creating an access card before trying to view it!");
                    return;
                }
            }

            userAccessCard = this.accessCards.find(e => e.Id == item.Id);

            if(!userAccessCard)
                throw new Error("WTF???");

            this.currentImage = userAccessCard.Image;
            this.selectedUser = item.Name;
            this.selectedUserId = item.Id;
            this.accessCardDisplayOverlay = !this.accessCardDisplayOverlay;
            this.getAccessCardDisplayState = true;
            this.createAccessCardDisplayState = false;
        },

        displayAlert(text: string)
        {
            this.alertText = text;

            this.alertDisplayState = true;
        },

        downloadAccessCard(image: string)
        {
            var a = document.createElement("a"); //Create <a>
            a.href = "data:image/png;base64," + image; //Image Base64 Goes here
            a.download = this.selectedUser + ".png"; //File name Here
            a.click(); //Downloaded file
        },

        toggleImageUploadForm(item: any)
        {
            this.selectedUser = item.Name;
            this.selectedUserId = item.Id;
            this.specificImageUpload = [];
            this.accessCardDisplayOverlay = !this.accessCardDisplayOverlay;
            this.createAccessCardDisplayState = true;
            this.getAccessCardDisplayState = false;
        },
    },
    watch:
    {
        searchName ()
        {
            this.search = String(Date.now())
        },
    },
}

</script>
<template>
    <v-expansion-panels multiple>
        <accessCardPanel title="Create Access Card" subTitle="Upload a picture">
            <v-card title="Single Image Upload">
                <v-form @submit.prevent class="pa-4">
                    <v-file-input
                    v-model="singleFileUpload"
                    clearable
                    label="Upload picture"
                    show-size
                    prepend-icon="mdi-camera"
                    accept="image/png, image/jpeg"
                    color="primary"
                    chips
                    ></v-file-input>
                    <v-btn
                    class="mt-2"
                    type="submit"
                    @click="createAccessCard()"
                    color="primary"
                    block
                    >Create</v-btn>
                </v-form>
            </v-card>

            <v-card title="Bulk Image Upload">
                <v-form @submit.prevent class="pa-4">
                    <v-file-input
                    v-model="bulkFileUpload"
                    clearable
                    label="Upload zip file"
                    show-size
                    prepend-icon="mdi-file"
                    accept=".zip,.rar,.7zip"
                    color="primary"
                    chips
                    ></v-file-input>
                    <v-btn
                    class="mt-2"
                    type="submit"
                    @click="bulkUploadImages()"
                    color="primary"
                    block
                    >Create</v-btn>
                </v-form>
            </v-card>
        </accessCardPanel>
            
        <accessCardPanel title="Get Access Card">
            <v-overlay
                v-model="accessCardDisplayOverlay"
                class="align-center justify-center"
                contained
            >
                <v-card :title="selectedUser" v-if="getAccessCardDisplayState">
                    <v-img
                    :width="300"
                    aspect-ratio="16/9"
                    cover
                    :src="`data:image/png;base64, ${currentImage}`"
                    class="ma-2"
                    ></v-img>
                    <v-card-actions>
                        <v-btn
                            class="ma-2"
                            variant="outlined"
                            color="cta"
                            @click="downloadAccessCard(currentImage)"
                        >
                            Download
                        </v-btn>
                        <v-btn
                            class="ma-2"
                            variant="outlined"
                            color="primary"
                            @click="accessCardDisplayOverlay = false"
                        >
                            Close
                        </v-btn>
                    </v-card-actions>
                </v-card>

                <!-- Form to upload image of specific user -->
                <v-card :title="'Create Access Card for ' + selectedUser" v-if="createAccessCardDisplayState">
                    <v-form @submit.prevent class="pa-4">
                        <v-file-input
                            v-model="specificImageUpload"
                            clearable
                            label="Upload picture"
                            show-size
                            prepend-icon="mdi-camera"
                            accept="image/png, image/jpeg"
                            color="primary"
                            chips
                        ></v-file-input>
                        <v-btn
                            class="mt-2"
                            type="submit"
                            @click="createSpecificAccessCard()"
                            color="primary"
                            block
                        >Create</v-btn>
                    </v-form>
                </v-card>
            </v-overlay>
            
            <v-data-table-server
            v-model:items-per-page="itemsPerPage"
            :headers="headers"
            :items="serverItems"
            :items-length="totalItems"
            :loading="loading"
            :search="search"
            item-value="name"
            @update:options="loadUsers"
            >
                <template #item.accessCards="{ item }"> <!-- #item.accessCards determines which key it falls under in the data table -->
                    <v-btn
                    @click="getAccessCard(item)"
                    color="primary"
                    >
                        Get
                    </v-btn>
                    <v-btn
                    @click="toggleImageUploadForm(item)"
                    class="ml-2"
                    color="cta"
                    >
                        Create
                    </v-btn>
                </template>
                <template v-slot:tfoot>
                <tr>
                    <td>
                    <v-text-field v-model="searchName" class="ma-2" density="compact" placeholder="Search name..." hide-details></v-text-field>
                    </td>
                </tr>
                </template>
            </v-data-table-server>
        </accessCardPanel>
    </v-expansion-panels>
    <Alert
        :text="alertText"
        :alertTime="alertTimeout"
        :displayState="alertDisplayState"
        @update:displayState="alertDisplayState = $event">
    </Alert>
</template>
<style scoped>
</style>