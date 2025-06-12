import { defineStore } from "pinia";
import { defaultApiURL } from "@/config";

export interface ITeacher {
  Id: number;
  Name: string;
  Email: string;
  Roles: string[];
}

const credential = "";

export const useManageRoleStore = defineStore("ManageRoleStore", {
  state: () => {
    return {
      teachers: [] as ITeacher[],
    };
  },
  actions:
  {
    /*
      Doesnt take in anything and doesnt return anything. Fetches data from backend and pushes it into teachers array
    */
    async fetchTeachers()
    {
      try
      {
        const response = await fetch(`${defaultApiURL}/api/ManageRoles`,
        {
          method: "GET",
          headers:
          {
            'Authorization': `Bearer ${credential}`,
            'Content-Type': 'application/json',
          }
        })
        if (!response.ok)
            throw new Error("Failed to get Teachers")

        this.teachers.splice(0, this.teachers.length);

        const data = await response.json();

        data.forEach(
          (element: {
            id: number;
            name: string;
            email: string;
            roles: string[];
          }) => {
            this.teachers.push({
              Id: element.id,
              Name: element.name,
              Email: element.email,
              Roles: element.roles,
            });
          },
        );
      } catch (error) {
        console.error("Error fetching Teachers: ", error);
      }
    },

    /*
      Takes in an ITeacher object and sends a request to the backend to update the role of a teacher. Doesn't return anything.
    */
    async updateRoles(teacher: ITeacher)
    {
      try
      {
        const response = await fetch(`${defaultApiURL}/api/ManageRoles/${teacher.Id}`,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(teacher),
          },
        );
      } catch (error) {
        console.error("Error updating Teachers: ", error);
      }
    },
  },
  getters: {
    allTeachers(): ITeacher[] {
      return this.teachers;
    },
  },
});
