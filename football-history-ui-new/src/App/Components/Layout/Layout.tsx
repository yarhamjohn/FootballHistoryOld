import { FC, ReactElement, ReactNode } from "react";
import { AppHeader } from "../AppHeader/AppHeader";
import { ActiveTab } from "../AppHeader/TabBar/TabBar";

type Props = {
  activeTab: ActiveTab;
  children?: ReactNode;
};

const Layout: FC<Props> = ({ children, activeTab }): ReactElement => {
  return (
    <>
      <AppHeader activeTab={activeTab} />
      {children}
    </>
  );
};

export { Layout };
