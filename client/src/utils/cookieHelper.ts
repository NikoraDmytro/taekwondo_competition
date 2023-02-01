export const setCookie = (
  cookieName: string,
  cookieValue: string,
  expirationType: "session" | "unlimited"
) => {
  const expires =
    expirationType === "unlimited"
      ? "expires=Fri, 31 Dec 9999 12:00:00 GMT;"
      : "";
  document.cookie = `${cookieName}=${cookieValue}; ${expires} path=/`;
};

export const removeCookie = (cookieName: string) => {
  document.cookie = `${cookieName}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/`;
};

export const getCookie = (cookieName: string) => {
  const name = cookieName + "=";
  const cookieArray = document.cookie.split(";");
  for (let i = 0; i < cookieArray.length; i++) {
    let cookie = cookieArray[i];
    while (cookie.charAt(0) === " ") {
      cookie = cookie.substring(1);
    }
    if (cookie.indexOf(name) === 0) {
      return cookie.substring(name.length, cookie.length);
    }
  }
  return null;
};
