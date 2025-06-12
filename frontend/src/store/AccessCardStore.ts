import { defineStore } from "pinia";
import { defaultApiURL } from "@/config";

export interface IUsers {
  Id: number;
  Name: string;
  Email: string;
}

interface IUserAccessCard {
  Id: number;
  Image: string;
}

export const useAccessCardStore = defineStore("AccessCardStore", {
    state: () => {
        return {
            users: [] as IUsers[],
            userAccessCard: [] as IUserAccessCard[],
        };
    },
    actions: {
        /*
            Takes image as arg and doesnt return anything. Sends image to backend for processing
        */
        async sendImage(image: any) {
            const file = image[0];

            if (!file) throw new Error("Please input file");

            const formData = new FormData();
            formData.append("file", file);
            try
            {
                const response = await fetch(`${defaultApiURL}/api/AccessCard/upload`,
                {
                    method: "POST",
                    body: formData,
                })

                if (!response.ok) throw new Error("Failed to post image");
            } catch (error) {
                console.error("Error posting image: ", error);
            }
        },

        /*
            Takes zipFile as arg and doesnt return anything. Sends zipFile to backend for processing
        */
        async sendBulkFile(zipFile: any)
        {
            const file = zipFile[0];

            if(!file)
                throw new Error("Please input file");

            const formData = new FormData();
            formData.append('zipFile', file);

            try
            {
                const response = await fetch(`${defaultApiURL}/api/AccessCard/upload/bulk`,
                {
                    method: "POST",
                    body: formData,
                })

                if (!response.ok)
                    throw new Error("Failed to post zipFile")

            } catch (error)
            {
                console.error("Error posting zipFile: ", error);
            }
        },

        /*
            Takes image and user id as arg and doesnt return anything. Sends image to backend for processing
        */
        async sendSpecificImage(image: any, id: number) {
            const file = image[0];

            if (!file) throw new Error("Please input file");

            const formData = new FormData();
            formData.append("file", file);

            try
            {
                const response = await fetch(`${defaultApiURL}/api/AccessCard/upload/${id}`,
                {
                    method: "POST",
                    body: formData,
                })

                if (!response.ok)
                    throw new Error("Failed to post image")

            } catch (error)
            {
                console.error("Error posting image: ", error);
            }
        },

        /*
            Fetches all users from backend
        */
        async fetchUsers()
        {
            try
            {
                const response = await fetch(`${defaultApiURL}/api/AccessCard/users`,
                {
                    headers:
                    {
                        'Content-Type': 'application/json'
                    }
                })
                if (!response.ok)
                    throw new Error("Failed to get Users")

                this.users.splice(0, this.users.length);

                const data = await response.json();

                data.forEach((element: { id: number; name: string; email: string; image?: string}) => {
                    this.users.push({
                        Id: element.id,
                        Name: element.name,
                        Email: element.email
                    })

                    if(element.image != null)
                    {
                        this.userAccessCard.splice(0, this.userAccessCard.length);
                        this.userAccessCard.push({
                            Id: element.id,
                            Image: element.image
                        })
                    }
                });

            } catch (error)
            {
                console.error("Error fetching Users: ", error);
            }
        },

        /* 
            Fetches an access card by user id.
        */
        async fetchAccessCardById(id: number)
        {
            try
            {
                const response = await fetch(`${defaultApiURL}/api/AccessCard/${id}`,
                {
                    method: "GET",
                    headers:
                    {
                        'Content-Type': 'application/json'
                    }
                })
                if (!response.ok)
                    return false;

                const data = await response.json();

                const userAccessCardIndex = this.userAccessCard.findIndex(UAC => UAC.Id == data.id)

                if(userAccessCardIndex != null)
                    this.userAccessCard.splice(userAccessCardIndex, 1)

                this.userAccessCard.push({
                    Id: data.id,
                    Image: data.image64Encoded,
                })

                return true;
            } catch (error)
            {
                console.error("Error fetching Access card: ", error);
                return false;
            }
        }
    },
    getters:
    {
        allUsers(): IUsers[] 
        {
            return this.users;
        }
    },
});
