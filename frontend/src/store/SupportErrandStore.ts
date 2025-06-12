import { defineStore } from "pinia";

interface ISupportErrand {
  id: string;
  name: string;
  title: string;
  description: string;
  createdAt: string;
  status: string;
}

export const useSupportErrandStore = defineStore("SupportErrandStore", {
  state: () => {
    return {
      SupportErrands: [] as ISupportErrand[],
    };
  },
  actions: {
    async fetchSupportErrands() {
      const data = [
        {
          id: "1",
          name: "Nicolai Zackrisson",
          title: "Broken Ipad",
          description: "I smashed my ipad with a hammer!",
          createdAt: "11:00:28",
          status: "Request",
        },
        {
          id: "2",
          name: "Linus Boström",
          title: "Computer not working",
          description: "My computer is not working!",
          createdAt: "21:10:28",
          status: "Accepted",
        },
        {
          id: "3",
          name: "William Fagher",
          title: "Keyboard doesn't work",
          description: "My keyboard is not working!",
          createdAt: "09:10:28",
          status: "Complete",
        },
        {
          id: "4",
          name: "Nicolai Zackrisson",
          title: "Broken Laptop",
          description: "I smashed my laptop with a hammer!",
          createdAt: "12:00:28",
          status: "Request",
        },
      ];
      this.SupportErrands = data;
    },
    filter(user: { name: string; admin: boolean }) {
      const filteredErrands: ISupportErrand[] = [];
      this.SupportErrands.forEach((errand) => {
        if (user.admin) {
          filteredErrands.push(errand);
        } else if (user.name === errand.name) {
          filteredErrands.push(errand);
        }
      });
      this.SupportErrands = filteredErrands;
    },
  },
  getters: {
    all(): ISupportErrand[] {
      return this.SupportErrands;
    },
  },
});
